// ResourceAmount.cs
// 
//  Modified MIT License (MIT)
//  
//  Copyright (c) 2015 Completely Fair Games Ltd.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// The following content pieces are considered PROPRIETARY and may not be used
// in any derivative works, commercial or non commercial, without explicit 
// written permission from Completely Fair Games:
// 
// * Images (sprites, textures, etc.)
// * 3D Models
// * Sound Effects
// * Music
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Diagnostics;

namespace DwarfCorp
{

    /// <summary>
    /// This is just a struct of two things: a resource tag, and a number of that resource.
    /// This is used instead of a list, since there is nothing distinguishing resources from each other.
    /// </summary>
    public class Quantitiy<T> : ICloneable
    {
        protected bool Equals(Quantitiy<T> other)
        {
            return Equals(Type, other.Type) && Count == other.Count;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((Quantitiy<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Type.GetHashCode() * 397) ^ Count;
            }
        }

        public virtual object Clone()
        {
            return new Quantitiy<T>(Type, Count);
        }

        public virtual Quantitiy<T> CloneQuantity()
        {
            return Clone() as Quantitiy<T>;
        }

        public T Type { get; set; }
        public int Count { get; set; }


        public Quantitiy()
        {

        }

        public Quantitiy(T type)
        {
            Type = type;
            Count = 1;
        }

        public Quantitiy(Quantitiy<T> other)
        {
            Type = other.Type;
            Count = other.Count;
        }

        public Quantitiy(T resourceType, int numResources)
        {
            Type = resourceType;
            Count = numResources;
        }


        public static Quantitiy<T> operator +(int a, Quantitiy<T> b)
        {
            return new Quantitiy<T>()
            {
                Type = b.Type,
                Count = b.Count + a
            };
        }

        public static Quantitiy<T> operator -(int b, Quantitiy<T> a)
        {
            return new Quantitiy<T>()
            {
                Type = a.Type,
                Count = a.Count - b
            };
        }

        public static Quantitiy<T> operator +(Quantitiy<T> a, int b)
        {
            return new Quantitiy<T>()
            {
                Type = a.Type,
                Count = a.Count + b
            };
        }

        public static Quantitiy<T> operator -(Quantitiy<T> a, int b)
        {
            return new Quantitiy<T>()
            {
                Type = a.Type,
                Count = a.Count - b
            };
        }

        public static bool operator ==(Quantitiy<T> a, Quantitiy<T> b)
        {
            if (ReferenceEquals(a, null) && !ReferenceEquals(b, null))
            {
                return true;
            }

            if (!ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return false;
            }

            if (ReferenceEquals(a, null))
            {
                return true;
            }

            return a.Type.Equals(b.Type) && (a.Count == b.Count);
        }

        public static bool operator !=(Quantitiy<T> a, Quantitiy<T> b)
        {
            return !(a == b);
        }

        public static bool operator <(Quantitiy<T> a, Quantitiy<T> b)
        {
            return (a.Type.Equals(b.Type)) && (a.Count < b.Count);
        }

        public static bool operator >(Quantitiy<T> a, Quantitiy<T> b)
        {
            return (a.Type.Equals(b.Type)) && (a.Count > b.Count);
        }
    }
}