//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.IO;
//using System.Windows.Forms;
//using System.Reflection;
//using ZachJohnson.Promptu.Collections;

//namespace ZachJohnson.Promptu.Skins
//{
//    internal static class SkinFinder
//    {
//        public static SkinCollection FindSkins()
//        {
//            SkinCollection skins = new SkinCollection();
//            string skinDirectory = Application.StartupPath + "\\Skins";
//            if (Directory.Exists(skinDirectory))
//            {
//                string[] files = Directory.GetFiles(skinDirectory);

//                foreach (string file in files)
//                {
//                    string extension = Path.GetExtension(file).ToUpperInvariant();
//                    if (extension == ".DLL" || extension == ".EXE")
//                    {
//                        Assembly assembly = null;
//                        try
//                        {
//                            assembly = Assembly.LoadFrom(file);
//                        }
//                        catch (BadImageFormatException)
//                        {
//                            continue;
//                        }

//                        if (assembly == null)
//                        {
//                            continue;
//                        }

//                        try
//                        {
//                            foreach (Type exportedType in assembly.GetExportedTypes())
//                            {
//                                Type iface = null;
//                                try
//                                {
//                                    iface = exportedType.GetInterface("IPrompt");
//                                }
//                                catch (AmbiguousMatchException)
//                                {
//                                    continue;
//                                }

//                                if (iface == null)
//                                {
//                                    continue;
//                                }

//                                try
//                                {
//                                    Skin skin = new Skin(exportedType);
//                                    skins.Add(skin);
//                                }
//                                catch (ArgumentException)
//                                {
//                                }
//                            }
//                        }
//                        catch (FileNotFoundException)
//                        {
//                        }
//                        catch (TypeLoadException)
//                        {
//                        }
//                    }
//                }
//            }

//            return skins;
//        }
//    }
//}
