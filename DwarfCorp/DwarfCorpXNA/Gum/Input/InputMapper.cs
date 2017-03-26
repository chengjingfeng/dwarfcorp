﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Gum.Input
{
    /// <summary>
    /// Translates windows keyboard messages into a form consumable by the GUI.
    /// </summary>
    public class GumInputMapper
    {
        public enum WindowMessage
        {
            WM_KEYDOWN = 0x100,
            WM_KEYUP = 0x101,
            WM_CHAR = 0x102,

            WM_MOUSEMOVE = 0x0200,
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x202,
            WM_RBUTTONDOWN = 0x204,
            WM_RBUTTONUP = 0x0205,
        };

        // Need a thread-safe queue of events, since these are generated by the windows message pump.

        public class QueuedInput
        {
            public Gum.InputEvents Message;
            public Gum.InputEventArgs Args;
        }

        public System.Threading.Mutex QueueLock = new System.Threading.Mutex();
        public List<QueuedInput> Queued = new List<QueuedInput>();

        private bool CtrlDown = false;
        private bool AltDown = false;
        private bool ShiftDown = false;

        public List<QueuedInput> GetInputQueue()
        {
            QueueLock.WaitOne();
            var r = Queued;
            Queued = new List<QueuedInput>();
            QueueLock.ReleaseMutex();
            return r;
        }

        public GumInputMapper(IntPtr WindowHandle)
        {
            MessageFilter.AddMessageFilter((c) => HandleEvent(c));
        }

        private bool HandleEvent(System.Windows.Forms.Message Msg)
        {
            QueueLock.WaitOne();
            bool handled = false;

            switch ((WindowMessage)Msg.Msg)
            {
                case WindowMessage.WM_CHAR:
                    {
                        var args = new System.Windows.Forms.KeyPressEventArgs((char)Msg.WParam);
                        Queued.Add(new QueuedInput
                        {
                            Message = Gum.InputEvents.KeyPress,
                            Args = new Gum.InputEventArgs
                            {
                                KeyValue = args.KeyChar,
                                Alt = false,
                                Control = false,
                                Shift = false,
                            }
                        });
                        handled = true;
                        break;
                    }
                case WindowMessage.WM_KEYDOWN:
                    {
                        var args = new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys)Msg.WParam);
                        
                        if (args.KeyData == System.Windows.Forms.Keys.Alt)
                            AltDown = true;
                        if (args.KeyData == System.Windows.Forms.Keys.ControlKey)
                            CtrlDown = true;
                        if (args.KeyData == System.Windows.Forms.Keys.ShiftKey)
                            ShiftDown = true;

                        var extended = ((int)Msg.LParam & 0x01000000) != 0;
                        var realCode = args.KeyCode;
                        if (realCode == System.Windows.Forms.Keys.ControlKey)
                            realCode = extended ? System.Windows.Forms.Keys.RControlKey : System.Windows.Forms.Keys.LControlKey;

                        Queued.Add(new QueuedInput
                        {
                            Message = Gum.InputEvents.KeyDown,
                            Args = new Gum.InputEventArgs
                            {
                                Alt = args.Alt,
                                Control = args.Control,
                                Shift = args.Shift,
                                KeyValue = (int)realCode,
                            }
                        });
                        handled = true;
                        break;
                    }
                case WindowMessage.WM_KEYUP:
                    {
                        var args = new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys)Msg.WParam);

                        if (args.KeyData == System.Windows.Forms.Keys.Alt)
                            AltDown = false;
                        if (args.KeyData == System.Windows.Forms.Keys.ControlKey)
                            CtrlDown = false;
                        if (args.KeyData == System.Windows.Forms.Keys.ShiftKey)
                            ShiftDown = false;

                        Queued.Add(new QueuedInput
                        {
                            Message = Gum.InputEvents.KeyUp,
                            Args = new Gum.InputEventArgs
                            {
                                Alt = args.Alt,
                                Control = args.Control,
                                Shift = args.Shift,
                                KeyValue = (int)args.KeyCode
                            }
                        });
                        handled = true;
                        break;
                    }
                case WindowMessage.WM_LBUTTONDOWN:
                    {
                        Queued.Add(new QueuedInput
                        {
                            Message = Gum.InputEvents.MouseDown,
                            Args = new Gum.InputEventArgs
                            {
                                Alt = AltDown,
                                Control = CtrlDown,
                                Shift = ShiftDown,
                                X = (int)((int)Msg.LParam & 0x0000FFFFu),
                                Y = (int)((int)Msg.LParam & 0xFFFF0000u) >> 16
                            }
                        });
                        handled = false;
                        break;
                    }
                case WindowMessage.WM_LBUTTONUP:
                    {
                        Queued.Add(new QueuedInput
                        {
                            Message = Gum.InputEvents.MouseUp,
                            Args = new Gum.InputEventArgs
                            {
                                Alt = AltDown,
                                Control = CtrlDown,
                                Shift = ShiftDown,
                                X = (int)((int)Msg.LParam & 0x0000FFFFu),
                                Y = (int)((int)Msg.LParam & 0xFFFF0000u) >> 16
                            }
                        });
                        Queued.Add(new QueuedInput
                        {
                            Message = Gum.InputEvents.MouseClick,
                            Args = new Gum.InputEventArgs
                            {
                                Alt = AltDown,
                                Control = CtrlDown,
                                Shift = ShiftDown,
                                X = (int)((int)Msg.LParam & 0x0000FFFFu),
                                Y = (int)((int)Msg.LParam & 0xFFFF0000u) >> 16
                            }
                        });
                        handled = false;
                        break;
                    }
                case WindowMessage.WM_MOUSEMOVE:
                    {
                        Queued.Add(new QueuedInput
                        {
                            Message = Gum.InputEvents.MouseMove,
                            Args = new Gum.InputEventArgs
                            {
                                Alt = AltDown,
                                Control = CtrlDown,
                                Shift = ShiftDown,
                                X = (int)((int)Msg.LParam & 0x0000FFFFu),
                                Y = (int)((int)Msg.LParam & 0xFFFF0000u) >> 16
                            }
                        });
                        handled = false;
                        break;
                    }
                default:
                    handled = false;
                    break;
            }

            QueueLock.ReleaseMutex();
            return handled;
        }

    }
}
