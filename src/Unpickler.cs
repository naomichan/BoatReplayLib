using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BoatReplayLib {
    public class Unpickler {
        public static object[] load(byte[] data) {
            using (MemoryStream ms = new MemoryStream(data)) {
                return load(ms);
            }
        }

        public class UnimplementedOpcode : NotImplementedException {
            public byte OpCode;
            public long Position;

            public UnimplementedOpcode(byte opcode, long position) : base($"Opcode {opcode:X} ({(char)opcode}) is unimplemented! Occured at 0x{position:X}") {
                OpCode = opcode;
                Position = position;
            }
        }

        public class PythonClass {
            public string Module;
            public string Name;
            public object Args;
            public object Dict;

            public PythonClass(string module, string name) {
                Module = module;
                Name = name;
            }
        }

        private static string readline(BinaryReader reader) {
            StringBuilder sb = new StringBuilder();
            char ch;
            while ((ch = reader.ReadChar()) != '\n') {
                sb.Append(ch);
            }
            return sb.ToString();
        }

		private const byte OPCODE_MARK = 0x28;
		private const byte OPCODE_EMPTY_TUPLE = 0x29;
        private const byte OPCODE_BINFLOAT = 0x47;
        private const byte OPCODE_INT = 0x49;
        private const byte OPCODE_BININT = 0x4A;
        private const byte OPCODE_BININT1 = 0x4B;
        private const byte OPCODE_BININT2 = 0x4D;
        private const byte OPCODE_SHORTBINSTRING = 0x55;
        private const byte OPCODE_BINUNICODE = 0x58;
		private const byte OPCODE_EMPTY_LIST = 0x5D;
        private const byte OPCODE_APPEND = 0x61;
		private const byte OPCODE_BUILD = 0x62;
		private const byte OPCODE_GLOBAL = 0x63;
        private const byte OPCODE_APPENDS = 0x65;
		private const byte OPCODE_BINGET = 0x68;
		private const byte OPCODE_LONGBINGET = 0x6A;
		private const byte OPCODE_OBJ = 0x6F;
        private const byte OPCODE_BINPUT = 0x71;
		private const byte OPCODE_LONGBINPUT = 0x72;
		private const byte OPCODE_SETITEM = 0x73;
		private const byte OPCODE_TUPLE = 0x74;
        private const byte OPCODE_SETITEMS = 0x75;
        private const byte OPCODE_EMPTY_DICT = 0x7D;
        private const byte OPCODE_PROTO = 0x80;
		private const byte OPCODE_NEWOBJ = 0x81;
		private const byte OPCODE_TUPLE1 = 0x85;
		private const byte OPCODE_TUPLE2 = 0x86;
		private const byte OPCODE_TUPLE3 = 0x87;
        private const byte OPCODE_NEWTRUE = 0x88;
        private const byte OPCODE_NEWFALSE = 0x89;
        private const byte OPCODE_STOP = 0x2E;

        private const string TRUE = "01";
        private const string FALSE = "00";

        public static object[] load(Stream data) {
            using (BinaryReader reader = new BinaryReader(data)) {
                Stack<object> stack = new Stack<object>();
                Dictionary<int, object> memo = new Dictionary<int, object>();
                Stack<Stack<object>> metastack = new Stack<Stack<object>>();

                // disable unused warning.
#pragma warning disable 0219
                byte protocol = 2;
#pragma warning restore 0219

                while (data.Position < data.Length) {
                    byte op = reader.ReadByte();
                    switch (op) {
                        case OPCODE_PROTO:
                            protocol = reader.ReadByte();
                            break;
						case OPCODE_EMPTY_LIST:
                            stack.Push(new List<object>());
							break;
						case OPCODE_EMPTY_TUPLE:
                            stack.Push(new object[0]{});
                            break;
						case OPCODE_EMPTY_DICT:
                            stack.Push(new Dictionary<object, object>());
							break;
						case OPCODE_SETITEMS: {
								Stack<object> items = stack;
								stack = metastack.Pop();
								Dictionary<object, object> dict = (Dictionary<object, object>)stack.Peek();
								while (items.Count > 0) {
									object a = items.Pop();
									object b = items.Pop();
									dict[b] = a;
								}
							}
							break;
						case OPCODE_SETITEM: {
                                object value = stack.Pop();
                                object key = stack.Pop();
                                Dictionary<object, object> dict = (Dictionary<object, object>)stack.Peek();
                                dict[key] = value;
							}
							break;
                        case OPCODE_BINPUT: {
                                byte i = reader.ReadByte();
                                memo[i] = stack.Peek();
                            }
                            break;
                        case OPCODE_LONGBINPUT: {
                                int i = reader.ReadInt32();
                                memo[i] = stack.Peek();
                            }
                            break;
                        case OPCODE_BINGET: {
                                byte i = reader.ReadByte();
                                stack.Push(memo[i]);
                            }
                            break;
                        case OPCODE_LONGBINGET: {
                                int i = reader.ReadInt32();
                                stack.Push(memo[i]);
                            }
                            break;
                        case OPCODE_MARK:
                            metastack.Push(stack);
                            stack = new Stack<object>();
                            break;
                        case OPCODE_BININT:
                            stack.Push(reader.ReadInt32());
                            break;
                        case OPCODE_BININT1:
                            stack.Push(reader.ReadByte());
							break;
						case OPCODE_BININT2:
							stack.Push(reader.ReadUInt16());
							break;
                        case OPCODE_BINFLOAT: 
                            stack.Push(BitConverter.ToDouble(reader.ReadBytes(8).Reverse().ToArray(), 0));
							break;
						case OPCODE_TUPLE1: {
								object a = stack.Pop();
								stack.Push(new object[1] { a });
							}
							break;
						case OPCODE_TUPLE2: {
								object a = stack.Pop();
								object b = stack.Pop();
								stack.Push(new object[2] { b, a });
							}
							break;
						case OPCODE_TUPLE3: {
								object a = stack.Pop();
								object b = stack.Pop();
								object c = stack.Pop();
								stack.Push(new object[3] { c, b, a });
							}
							break;
                        case OPCODE_SHORTBINSTRING: {
                                byte length = reader.ReadByte();
                                stack.Push(Encoding.ASCII.GetString(reader.ReadBytes(length)));
                            }
                            break;
                        case OPCODE_BINUNICODE: {
                                int length = reader.ReadInt32();
                                stack.Push(Encoding.UTF8.GetString(reader.ReadBytes(length)));
                            }
                            break;
                        case OPCODE_NEWTRUE:
                            stack.Push(true);
							break;
						case OPCODE_NEWFALSE:
							stack.Push(false);
							break;
                        case OPCODE_TUPLE: {
								Stack<object> items = stack;
								stack = metastack.Pop();
                                stack.Push(items.ToArray());
                            }
                            break;
                        case OPCODE_BUILD: {
								object state = stack.Pop();
                                PythonClass inst = stack.Peek() as PythonClass;
                                inst.Dict = state;
                            }
                            break;
                        case OPCODE_GLOBAL: {
                                string module = readline(reader);
                                string name = readline(reader);
                                stack.Push(new PythonClass(module, name));
							}
							break;
                        case OPCODE_INT: {
                                string line = readline(reader);
                                if (line == TRUE) {
                                    stack.Push(true);
                                } else if (line == FALSE) {
                                    stack.Push(false);
                                } else {
                                    stack.Push(long.Parse(line));
                                }
                            }
							break;
						case OPCODE_APPENDS: {
								Stack<object> items = stack;
								stack = metastack.Pop();
								List<object> list = (List<object>)stack.Peek();
								while (items.Count > 0) {
									list.Add(items.Pop());
								}
							}
							break;
						case OPCODE_APPEND: {
                                object item = stack.Pop();
								List<object> list = (List<object>)stack.Peek();
								list.Add(item);
							}
							break;
						case OPCODE_NEWOBJ: {
								object args = stack.Pop();
								object cls = stack.Pop();
								PythonClass klass = cls as PythonClass;
								klass.Args = args;
								stack.Push(klass);
							}
							break;
						case OPCODE_OBJ: {
                                Stack<object> args = stack;
                                stack = metastack.Pop();
								object cls = args.Pop();
								PythonClass klass = cls as PythonClass;
                                klass.Args = args.ToList();
								stack.Push(klass);
							}
							break;
                        case OPCODE_STOP:
                            object[] ret = stack.ToArray();
                            metastack.Clear();
                            stack.Clear();
                            memo.Clear();
                            return ret;
                        default:
                            throw new UnimplementedOpcode(op, data.Position);
                    }
                }
                throw new Exception("Never recieved STOP");
            }
        }
    }
}
