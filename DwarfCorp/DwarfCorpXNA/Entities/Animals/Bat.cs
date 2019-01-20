using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DwarfCorp;
using DwarfCorp.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace DwarfCorp
{
    public class Bat : Creature
    {
        [EntityFactory("Bat")]
        private static GameComponent __factory(ComponentManager Manager, Vector3 Position, Blackboard Data)
        {
            return new Bat(Manager, Position);
        }

        public Bat()
        {

        }

        public Bat(ComponentManager manager, Vector3 position) :
            base
            (
                manager,
                new CreatureStats
                {
                    Dexterity = 6,
                    Constitution = 1,
                    Strength = 1,
                    Wisdom = 1,
                    Charisma = 1,
                    Intelligence = 1,
                    Size = 0.25f,
                    CanSleep = false
                },
                "Carnivore",
                manager.World.PlanService,
                manager.World.Factions.Factions["Carnivore"],
                "Bat"
            )
        {
            Physics = new Physics
                (
                manager,
                    "bat",
                    Matrix.CreateTranslation(position),
                    new Vector3(0.375f, 0.375f, 0.375f),
                    new Vector3(0.0f, 0.0f, 0.0f),
                    1.0f, 1.0f, 0.999f, 0.999f,
                    new Vector3(0, -10, 0)
                );

            Physics.AddChild(this);

            // When true, causes the bird to face the direction its moving in
            Physics.Orientation = Physics.OrientMode.RotateY;

            CreateCosmeticChildren(Manager);

            // Used to sense hostile creatures
            Physics.AddChild(new EnemySensor(Manager, "EnemySensor", Matrix.Identity, new Vector3(20, 5, 20), Vector3.Zero));

            // Controls the behavior of the creature
            Physics.AddChild(new BatAI(Manager, "Bat AI", Sensors));
            AI.Movement.CanFly = true;
            AI.Movement.CanSwim = false;
            AI.Movement.CanClimb = false;
            AI.Movement.CanWalk = false;

            // The bird can peck at its enemies (0.1 damage)
            Attacks = new List<Attack> { new Attack("Bite", 0.01f, 2.0f, 1.0f, ContentPaths.Audio.Oscar.sfx_oc_bat_attack_1, ContentPaths.Effects.bite) { TriggerMode = Attack.AttackTrigger.Animation, TriggerFrame = 1, Mode = Attack.AttackMode.Dogfight, DiseaseToSpread = "Rabies" } };


            // The bird can hold one item at a time in its inventory
            Physics.AddChild(new Inventory(Manager, "Inventory", Physics.BoundingBox.Extents(), Physics.LocalBoundingBoxOffset));

            // The bird will emit a shower of blood when it dies
            Physics.AddChild(new ParticleTrigger("blood_particle", Manager, "Death Gibs", Matrix.Identity, Vector3.One, Vector3.Zero)
            {
                TriggerOnDeath = true,
                TriggerAmount = 1
            });

            // The bird is flammable, and can die when exposed to fire.
            Physics.AddChild(new Flammable(Manager, "Flames"));

            // Tag the physics component with some information 
            // that can be used later
            Physics.Tags.Add("Bat");
            Physics.Tags.Add("Animal");

            Stats.FullName = TextGenerator.GenerateRandom("$firstname") + " the bat";
            Stats.CurrentClass = SharedClass;


            Species = "Bat";
            CanReproduce = true;
            BabyType = "Bat";
        }

        public override void CreateCosmeticChildren(ComponentManager manager)
        {
            Stats.CurrentClass = SharedClass;
            CreateSprite(ContentPaths.Entities.Animals.Bat.bat_animations, manager, 0.0f);
            Physics.AddChild(Shadow.Create(0.3f, manager));
            NoiseMaker = new NoiseMaker();
            NoiseMaker.Noises["Hurt"] = new List<string>() { ContentPaths.Audio.Oscar.sfx_oc_bat_hurt_1 };
            NoiseMaker.Noises["Chirp"] = new List<string>() { ContentPaths.Audio.Oscar.sfx_oc_bat_neutral_1, ContentPaths.Audio.Oscar.sfx_oc_bat_neutral_2 };
            base.CreateCosmeticChildren(manager);
        }

        private static EmployeeClass SharedClass = new EmployeeClass()
        {
            Name = "Bat",
                Levels = new List<EmployeeClass.Level>() { new EmployeeClass.Level() { Index = 0, Name = "Bat" } },
            };
    }


    /// <summary>
    /// Extends CreatureAI specifically for
    /// bat behavior.
    /// </summary>
    public class BatAI : CreatureAI
    {
        public BatAI()
        {

        }

        public BatAI(ComponentManager Manager, string name, EnemySensor sensor) :
            base(Manager, name, sensor)
        {

        }

        IEnumerable<Act.Status> ChirpRandomly()
        {
            Timer chirpTimer = new Timer(MathFunctions.Rand(6f, 10f), false);
            while (true)
            {
                chirpTimer.Update(DwarfTime.LastTime);
                if (chirpTimer.HasTriggered)
                    Creature.NoiseMaker.MakeNoise(ContentPaths.Audio.bunny, Creature.AI.Position, true, 0.01f);
                yield return Act.Status.Running;
            }
        }


        // Overrides the default ActOnIdle so we can
        // have the bird act in any way we wish.
        public override Task ActOnIdle()
        {
            return new ActWrapperTask(
                new Parallel(new FlyWanderAct(this, 10.0f + MathFunctions.Rand() * 2.0f, 2.0f + MathFunctions.Rand() * 0.5f, 20.0f, 4.0f + MathFunctions.Rand() * 2, 10.0f) { CanPerchOnGround =  false, CanPerchOnWalls = true}
                , new Wrap(ChirpRandomly)) { ReturnOnAllSucces = false, Name = "Fly" });
        }
    }
}
