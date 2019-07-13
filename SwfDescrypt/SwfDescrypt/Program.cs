using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
//using SevenZip;

namespace SwfDescrypt
{
    class Program
    {
        static PacketReader2 packet;
        static byte[] key;

        public static int BUFFER_SIZE = 4096;

        static void Main(string[] args)
        {
            Console.Title = "Descrypt";
            string ndir = AppDomain.CurrentDomain.BaseDirectory;

            JavaScriptSerializer jsonSerialiser = new JavaScriptSerializer();

            if (args.Length > 0)
            {

                Console.WriteLine("l {0}", args[0]);

                string filename = Path.GetFileNameWithoutExtension(args[0]);
                string type = Path.GetExtension(args[0]);
                byte[] data = File.getBytes(args[0]);     

                Console.WriteLine("type: {0}", type);
                if (type.Equals(".png"))
                {
                    string namef = "img-PNG32";
                    string filex = filename.Replace(".swf", "");
                    filex = filex + ".swf";
                    PacketWriter2 writer2 = new PacketWriter2();
                    writer2.WriteI((short)namef.Length);
                    writer2.WriteASCIIFixed(namef, namef.Length);
                    writer2.Write(data, 0, data.Length);
                    Encoder encoder = new Encoder();
                    byte[] enc = encoder.encrypt(writer2.ToArray());
                    ByteArrayToFile(ndir + filex, enc);
                } else if (type.Equals(".json"))
                {

                    using (StreamReader sr = new StreamReader(args[0]))
                    {
                        // Read the stream to a string, and write the string to the console.
                        String line = sr.ReadToEnd();
                        Console.WriteLine(line);

                        List<TXT> __txt = jsonSerialiser.Deserialize<List<TXT>>(line);

                        __txt.ForEach(delegate (TXT item)
                        {
                            Console.WriteLine("ID: {0} Name: {1}", item.Id, item.Name);
                        });

                        Console.ReadKey();
                    }

                }
                return;
            }

            


            string[] array2 = Directory.GetFiles(ndir, "*.swf");
            foreach (var dir in array2)
            {
                string filename = Path.GetFileName(dir);
                byte[] data = File.getBytes(dir);

                Encoder encoder = new Encoder();
                byte[] data_de = encoder.decrypt(data);

                if (filename.Equals("text.swf"))
                {

                }

                if (filename.Equals("90011.swf"))
                {
                    Stream stream = new MemoryStream(data_de);
                    LzmaDecode lzmaDecode = new LzmaDecode(stream);
                    byte[] data_f = ReadFully(lzmaDecode);
                    PacketReader2 param1 = new PacketReader2(data_f, data_f.Length);
                    Console.WriteLine("l: {0}", data_f.Length);

                    byte[] data_0 = param1.ReadBytes(119852);

                    ByteArrayToFile(ndir + "data_0" + ".bin", data_0);



                    /*//TextureCenter
                    List<TextureCenter> __texturecenter = new List<TextureCenter>();
                    int _loc3_ = param1.ReadInt16();
                    while (_loc3_ > 0)
                    {
                        int ID = param1.ReadInt16();
                        int resID = param1.ReadInt32();
                        __texturecenter.Add(new TextureCenter()
                        {
                            ID = (short)ID,
                            resID = resID
                        });
                        _loc3_--;
                    }
                    
                    string json = jsonSerialiser.Serialize(__texturecenter);
                    System.IO.File.WriteAllText(ndir + "texturecenter.json", json);

                    //FilterCenter
                    int _loc2_ = param1.ReadIInt32();
                    byte[] ___data = param1.ReadBytes(_loc2_);
                    ByteArrayToFile(ndir + "filtercenter" + ".bin", ___data);
                    PacketReader2 bytes = new PacketReader2(___data, ___data.Length);
                    Console.WriteLine("totalr: {0}", bytes.ReadInt16());


                    //ModleCenter
                    List<ModleCenter> modlecenter = new List<ModleCenter>();
                    _loc3_ = param1.ReadInt16();
                    while (_loc3_ > 0)
                    {
                        int ID = param1.ReadInt16();
                        int resID = param1.ReadInt32();
                        int actionID = param1.ReadInt16();
                        bool hasAnimate = param1.ReadByte() == 0x00 ?  false : true;
                        modlecenter.Add(new ModleCenter()
                        {
                            ID = (short)ID,
                            resID = resID,
                            actionID = (short)actionID,
                            hasAnimate = hasAnimate
                        });
                        _loc3_--;
                    }
                    string json2x = jsonSerialiser.Serialize(modlecenter);
                    System.IO.File.WriteAllText(ndir + "modlecenter.json", json2x);

                    //EquipEffectCenter
                    _loc2_ = param1.ReadInt16();
                    while (_loc2_ > 0)
                    {
                        int ID = param1.ReadInt16();
                        int tmpp = (int)param1.ReadByte();
                        short[] filterIDs = new short[tmpp];
                        int __tmp0 = 0;
                        while (__tmp0 < tmpp)
                        {
                            filterIDs[__tmp0] = param1.ReadInt16();
                            __tmp0++;
                        }
                        _loc2_--;
                    }

                    //MapCenter
                    _loc3_ = param1.ReadInt16();
                    while (_loc3_ > 0)
                    {
                        int ID = param1.ReadInt16();
                        int modleID = param1.ReadInt16();
                        int resID = param1.ReadInt32();
                        int topX = param1.ReadInt32();
                        int topZ = param1.ReadInt32();
                        int width = param1.ReadInt32();
                        int height = param1.ReadInt32();
                        _loc3_--;
                    }

                    //AniCenter
                    _loc3_ = param1.ReadInt16();
                    while (_loc3_ > 0)
                    {
                        int ID = param1.ReadInt16();
                        int resID = param1.ReadInt32();
                        byte effectType = param1.ReadByte();
                        _loc3_--;
                    }

                    //ScriptCenter
                    _loc3_ = param1.ReadInt16();
                    while (_loc3_ > 0)
                    {
                        int ID = param1.ReadInt16();
                        int resID = param1.ReadInt32();
                        _loc3_--;
                    }

                    //SoundCenter
                    _loc3_ = param1.ReadInt16();
                    while (_loc3_ > 0)
                    {
                        int ID = param1.ReadInt16();
                        int resID = param1.ReadInt32();
                        _loc3_--;
                    }

                    //V
                    int _loc4_ = param1.ReadInt32();
                    while (_loc4_ > 0)
                    {
                        _loc2_ = param1.ReadInt32();
                        _loc3_ = param1.ReadByte();
                        _loc4_--;
                    }

                    //int __total = _loc4_ * 5;

                    //byte[] __v = param1.ReadBytes(__total);
                    //Utils.HexDump(__v, "");


                    //UICenter
                    {
                        //TXT

                        _loc2_ = param1.ReadInt16();
                        Console.WriteLine("TXT: {0}", _loc2_);
                        List<TXT> __datos = new List<TXT>();
                        while (_loc2_ > 0)
                        {
                            int a0 = param1.ReadInt16();
                            int lng = (int)param1.ReadInt16();
                            byte[] dd = param1.ReadString(lng, true);
                            Encoding utf_8 = Encoding.UTF8;
                            string s_unicode2 = Encoding.UTF8.GetString(dd);
                            __datos.Add(new TXT() {
                                Id = (short)a0,
                                Name = s_unicode2
                            });
                            _loc2_--;
                        }
                        string json2 = jsonSerialiser.Serialize(__datos);
                        System.IO.File.WriteAllText(ndir + "txt.json", json2);

                        int sobra = (param1.Length - param1.Index);
                        byte[] sobra3 = param1.ReadBytes(sobra);

                        ByteArrayToFile(ndir + "sobra3" + ".bin", sobra3);

                        //TXT.deser(param1);
                        //var _loc7_:int = param1.readByte();
                        //while (_loc7_ > 0)
                        //{
                        //    Logger.newObject(new FontEntity());
                        //    _loc4_ = new FontEntity();
                        //    _loc4_.deser(param1);
                        //    fonts[_loc4_.ID] = _loc4_;
                        //    _loc7_--;
                        //}
                        //_loc7_ = param1.readShort();
                        //while (_loc7_ > 0)
                        //{
                        //    _loc6_ = param1.readByte();
                        //    param1.position = param1.position - 1;
                        //    _loc5_ = UIConst.getFaceRecordByID(_loc6_);
                        //    Logger.newObject(new _loc5_.cls());
                        //    _loc2_ = new _loc5_.cls();
                        //    _loc2_.deser(param1);
                        //    facees[_loc2_.ID] = _loc2_;
                        //    _loc7_--;
                        //}
                        //_loc7_ = param1.readShort();
                        //while (_loc7_ > 0)
                        //{
                        //    Logger.newObject(new FormEntity());
                        //    _loc3_ = new FormEntity();
                        //    _loc3_.deser(param1);
                        //    forms[_loc3_.ID] = _loc3_;
                        //    _loc7_--;
                        //}
                    }*/

                    //ByteArrayToFile(ndir + filename + ".bin", data_f);
                    Console.WriteLine("Compelte");
                    Console.ReadKey();
                    return;
                }

                Encoding enc = Encoding.ASCII;
                packet = new PacketReader2(data_de, data_de.Length);
                packet.ReadByte();
                int nlen = packet.ReadByte();
                string nname = enc.GetString(packet.ReadBytes(nlen));
                string nf = nname.Split('-')[0];

                /*Utils.HexDump(data_de, "");
                Console.ReadKey();*/

                if (nf == "img")
                {
                    if (!Directory.Exists(ndir + "/res/img/"))
                    {
                        Directory.CreateDirectory(ndir + "/res/img/");
                    }
                    ByteArrayToFile(ndir + "/res/img/" + Path.GetFileName(dir) + ".png", packet.ReadBytes(data_de.Length - nlen - 2));
                }
                else if (nf == "modle")
                {
                    if (!Directory.Exists(ndir + "/res/modle/"))
                    {
                        Directory.CreateDirectory(ndir + "/res/modle/");
                    }
                    ByteArrayToFile(ndir + "/res/modle/" + Path.GetFileName(dir) + ".bin", packet.ReadBytes(data_de.Length - nlen - 2));
                }
                else if (nf == "script") {
                    if (!Directory.Exists(ndir + "/res/script/"))
                    {
                        Directory.CreateDirectory(ndir + "/res/script/");
                    }
                    ByteArrayToFile(ndir + "/res/script/" + Path.GetFileName(dir) + ".script", packet.ReadBytes(data_de.Length - nlen - 2));
                }
                else if (nf == "ani")
                {
                    if (!Directory.Exists(ndir + "/res/ani/"))
                    {
                        Directory.CreateDirectory(ndir + "/res/ani/");
                    }
                    ByteArrayToFile(ndir + "/res/ani/" + Path.GetFileName(dir) + ".ani", packet.ReadBytes(data_de.Length - nlen - 2));
                }
                else if (nf == "texture")
                {
                    if (!Directory.Exists(ndir + "/res/texture/"))
                    {
                        Directory.CreateDirectory(ndir + "/res/texture/");
                    }
                    ByteArrayToFile(ndir + "/res/texture/" + Path.GetFileName(dir) + ".texture", packet.ReadBytes(data_de.Length - nlen - 2));
                }
                else if (nf == "map")
                {
                    if (!Directory.Exists(ndir + "/res/map/"))
                    {
                        Directory.CreateDirectory(ndir + "/res/map/");
                    }
                    ByteArrayToFile(ndir + "/res/map/" + Path.GetFileName(dir) + ".map", packet.ReadBytes(data_de.Length - nlen - 2));
                }
                else if (nf == "sound")
                {
                    if (!Directory.Exists(ndir + "/res/sound/"))
                    {
                        Directory.CreateDirectory(ndir + "/res/sound/");
                    }
                    ByteArrayToFile(ndir + "/res/sound/" + Path.GetFileName(dir) + ".sound", packet.ReadBytes(data_de.Length - nlen - 2));
                }
                else if (nlen < 20)
                {
                    Console.WriteLine("dir: {0}", dir);
                    Console.WriteLine("nlen: {0}", nlen);
                    Console.WriteLine("nname: {0}", nname);
                    Console.WriteLine("nf: {0}", nf);
                }
            }

            //string nname = enc.GetString(packet.ReadBytes(nlen));
            //Console.WriteLine("nname: {0}", nname);

            

            //Utils.HexDump(Decompress(_loc3_), "");


            /*try
            {
                Utils.HexDump(Decompress(_loc3_), "");
            } catch(Exception e)
            {
                Console.WriteLine("unpack");
            }*/


            //ByteArrayToFile("game222.swf", _loc3_);*/


            /*String file = "game222.swf";
            byte[] data = File.getBytes(file);
            packet = new PacketReader2(data, data.Length);
            int cfiles = (int)packet.ReadInt16();
            int counter = 0;

            Console.WriteLine("files: {0}", cfiles);

            while(counter < cfiles)
            {
                int namestr = (int)packet.ReadByte();
                Console.WriteLine("namestr: {0}", namestr);
                string name = packet.ReadString(namestr);
                Console.WriteLine("file: {0}", name);
                byte[] b = packet.ReadBytes(4);
                Array.Reverse(b);
                int size = BitConverter.ToInt32(b, 0);
                Console.WriteLine("size: {0}", size);
                byte[] tmp_data = packet.ReadBytes(size);
                ByteArrayToFile(name, tmp_data);
                packet.ReadByte();
                counter++;
            }*/



            //Console.ReadKey();
        }


        public static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }


        public static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }

        static int ReverseBytes(int val)
        {
            byte[] intAsBytes = BitConverter.GetBytes(val);
            Array.Reverse(intAsBytes);
            return BitConverter.ToInt32(intAsBytes, 0);
        }
    }
}
