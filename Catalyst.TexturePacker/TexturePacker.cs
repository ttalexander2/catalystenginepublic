using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Catalyst.Engine.Rendering;
using Catalyst.Engine.Utilities;
using Rectangle = System.Drawing.Rectangle;

namespace Catalyst.ContentManager
{
    public static class TexturePacker
    {
        public static bool Verbose = false;
        public static bool Force = false;
        public static bool Trim = false;
        public static bool Xml = false;
        
        public static bool PackAtlas(string input_directory, string output_directory, string output_name, params string[] args)
        {

            foreach (string a in args)
            {
                if (a == "-v" || a == "--verbose")
                {
                    Verbose = true;
                    continue;
                }
                if (a == "-f" || a == "--force")
                {
                    Force = true;
                    continue;
                }
                if (a == "-t" || a == "--trim")
                {
                    Trim = true;
                    continue;
                }
                if (a == "-x" || a == "--XML" || a == "--xml")
                {
                    Xml = true;
                    continue;
                }
            }

            if (Verbose)
            {
                Log.WriteLine(String.Format("Arguemts: Verbose[{0}], Force Recompilation[{1}], Trim Sprites[{2}], XML instead of binary[{3}]", Verbose, Force, Trim, Xml));
            }

            byte[] hash = HashDirectory(input_directory);

            if (!Directory.Exists(output_directory))
            {
                try
                {
                    Directory.CreateDirectory(output_directory);
                } catch (Exception)
                {
                    throw new TexturePackerException(String.Format("Failed to create directory: {0}", output_directory));
                }

            }
            if (File.Exists(Path.Combine(output_directory, output_name + ".hash")))
            {
                if(Verbose)
                    Log.WriteLine("Checking hash file...");
                byte[] previous_hash = File.ReadAllBytes(Path.Combine(output_directory, output_name + ".hash"));

                if (Verbose)
                {
                    Log.WriteLine(String.Format("Previous Directory Hash:\t{0}", BitConverter.ToString(previous_hash).Replace("-", "").ToLower()));
                    Log.WriteLine(String.Format("Current Directory Hash:\t\t{0}", BitConverter.ToString(hash).Replace("-", "").ToLower()));
                }

                if (BitConverter.ToString(hash).Replace("-", "").ToLower() == BitConverter.ToString(previous_hash).Replace("-", "").ToLower())
                {
                    if (!Force)
                    {
                        if (Verbose)
                            Log.WriteLine("The texture atlas has not been changed");
                        return false;
                    }
                }
            }

            using (FileStream fs = new FileStream(Path.Combine(output_directory, output_name + ".hash"), FileMode.Create))
            {
                fs.Write(hash, 0, hash.Length);
            }

            List<string> files = Directory.GetFiles(input_directory, "*.png", SearchOption.TopDirectoryOnly).OrderBy(p => p).ToList();

            List<Bitmap> bitmaps = new List<Bitmap>();

            int total_area = 0;
            int max_height = 0;

            //For non-animated textures
            foreach(string file in files)
            {
                if (Verbose)
                    Log.WriteLine("Reading image from: " + file);
                byte[] imageData = File.ReadAllBytes(file);
                using (var ms = new MemoryStream(imageData))
                {
                    Bitmap b = new Bitmap(ms);
                    /**
                    if (Trim)
                        b = TrimBitmap(b);
                    **/
                    b.Tag = Path.GetFileNameWithoutExtension(file);
                    bitmaps.Add(b);
                    total_area += b.Width * b.Height;
                    max_height += b.Height;
                }
            }

            //For animated textures

            foreach (string directory in Directory.GetDirectories(input_directory).OrderBy(p => p).ToList())
            {
                foreach (string file in Directory.GetFiles(directory, "*.png", SearchOption.TopDirectoryOnly))
                {
                    if (Verbose)
                        Log.WriteLine("Reading animated image from: " + file);
                    byte[] imageData = File.ReadAllBytes(file);
                    using (var ms = new MemoryStream(imageData))
                    {
                        Bitmap b = new Bitmap(ms);
                        /**
                        if (Trim)
                            b = TrimBitmap(b);
                        **/
                        b.Tag = $"animated1597534568919817981981_{Path.GetFileNameWithoutExtension(file)}";
                        bitmaps.Add(b);
                        total_area += b.Width * b.Height;
                        max_height += b.Height;
                    }
                }

            }

            bitmaps = bitmaps.OrderByDescending(p => p.Height).ToList();

            BinaryTreePacker packed = new BinaryTreePacker(total_area / bitmaps.Count / 4, max_height, 0, true);

            foreach(Bitmap b in bitmaps)
            {
                packed.Insert(b);
            }

            Bitmap atlas = new Bitmap(total_area / bitmaps.Count / 4, max_height);

            if (Verbose)
            {
                Log.WriteLine("Atlas contains " + bitmaps.Count + " total images.");
            }



            atlas = DrawPackedNodes(atlas, packed.Root);

            atlas = TrimBitmap(atlas);

            atlas.Save(Path.Combine(output_directory, output_name + ".data"), ImageFormat.Png);

            if (Xml)
            {
                WriteMetaToXml(packed.Root, Path.Combine(output_directory, output_name + ".meta"));
            }
            else
            {
                WriteMetaToBinary(packed.Root, Path.Combine(output_directory, output_name + ".meta"));
            }

            return true;

        }

        private static Bitmap DrawPackedNodes(Bitmap atlas, PackedNode root)
        {
            if (root == null)
            {
                return atlas;
            }
            using (var g = System.Drawing.Graphics.FromImage(atlas))
            {
                if (root.Image != null)
                    g.DrawImage(root.Image, root.Rect);
            }
            atlas = DrawPackedNodes(atlas, root.Left);
            atlas = DrawPackedNodes(atlas, root.Right);
            return atlas;
        }

        private static void WriteMetaToBinary(PackedNode root, string file_output)
        {
            BinaryWriter writer = new BinaryWriter(new FileStream(file_output, FileMode.Create));
            WriteTreeBinary(root, writer);
            writer.Close();
        }

        private static void WriteTreeBinary(PackedNode root, BinaryWriter writer)
        {
            if (root == null)
                return;
            if (root.Image != null)
            {
                if (root.Image.Tag is string)
                {
                    
                    if (((string)root.Image.Tag).StartsWith("animated1597534568919817981981_"))
                    {
                        writer.Write(((string)root.Image.Tag).TrimStart("animated1597534568919817981981_".ToCharArray()));
                        writer.Write(true);
                    } 
                    else
                    {
                        writer.Write(((string)root.Image.Tag));
                        writer.Write(false);
                    }
                        
                }
                else
                    writer.Write("no string tag");
                writer.Write(root.Rect.X);
                writer.Write(root.Rect.Y);
                writer.Write(root.Rect.Width);
                writer.Write(root.Rect.Height);
                writer.Write(root.Rotate);
            }

            WriteTreeBinary(root.Right, writer);
            WriteTreeBinary(root.Left, writer);

        }

        public static Atlas AtlasFromBinary(string atlas_file, string meta_file)
        {
            if (!File.Exists(atlas_file))
            {
                throw new TexturePackerException(String.Format("Cannot read. Atlas file {0} does not exist!", atlas_file));
            }

            if (!File.Exists(meta_file))
            {
                throw new TexturePackerException(String.Format("Cannot read. Atlas meta file {0} does not exist!", meta_file));
            }

            Atlas atlas = new Atlas(atlas_file);
            atlas.LoadContent();

            using (BinaryReader reader = new BinaryReader(new FileStream(meta_file, FileMode.Open)))
            {
                int count = 0;
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    string tag = reader.ReadString();
                    bool animated = reader.ReadBoolean();
                    int x = reader.ReadInt32();
                    int y = reader.ReadInt32();
                    int w = reader.ReadInt32();
                    int h = reader.ReadInt32();
                    bool rotate = reader.ReadBoolean();
                    Catalyst.Engine.Rendering.PackedTexture t = new Catalyst.Engine.Rendering.PackedTexture(count, tag, atlas, new Catalyst.Engine.Utilities.Rectangle(x, y, w, h), rotate);
                    atlas.Textures.Add(count, t);
                    count++;
                }
            }

            return atlas;
        }

        private static void WriteMetaToXml(PackedNode root, string file_output)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(new FileStream(file_output, FileMode.Create), settings);
            writer.WriteStartElement("Atlas");
            WriteTreeXml(root, writer);
            writer.WriteEndElement();
            writer.Close();
        }

        private static void WriteTreeXml(PackedNode root, XmlWriter writer)
        {
            if (root == null)
                return;
            if (root.Image != null)
            {
                writer.WriteStartElement("Image");
                if (root.Image.Tag is string)
                {
                    if (((string)root.Image.Tag).StartsWith("animated1597534568919817981981_"))
                    {
                        writer.WriteAttributeString("Tag", ((string)root.Image.Tag).TrimStart("animated1597534568919817981981_".ToCharArray()));
                        writer.WriteStartElement("Animated");
                        writer.WriteValue(true);
                        writer.WriteEndElement();
                    }
                    else
                    {
                        writer.WriteAttributeString("Tag", ((string)root.Image.Tag));
                        writer.WriteStartElement("Animated");
                        writer.WriteValue(false);
                        writer.WriteEndElement();
                    }
                }
                else
                {
                    writer.WriteAttributeString("Tag", "None");
                    writer.WriteStartElement("Animated");
                    writer.WriteValue(false);
                    writer.WriteEndElement();
                }
                    
               
                writer.WriteStartElement("X");
                writer.WriteValue(root.Rect.X);
                writer.WriteEndElement();
                writer.WriteStartElement("Y");
                writer.WriteValue(root.Rect.Y);
                writer.WriteEndElement();
                writer.WriteStartElement("Width");
                writer.WriteValue(root.Rect.Width);
                writer.WriteEndElement();
                writer.WriteStartElement("Height");
                writer.WriteValue(root.Rect.Height);
                writer.WriteEndElement();
                writer.WriteStartElement("Rotate");
                writer.WriteValue(root.Rotate);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            WriteTreeXml(root.Left, writer);
            WriteTreeXml(root.Right, writer);

        }

        private static Bitmap TrimBitmap(Bitmap source)
        {
            System.Drawing.Rectangle srcRect = default(System.Drawing.Rectangle);
            BitmapData data = null;
            try
            {
                data = source.LockBits(new System.Drawing.Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] buffer = new byte[data.Height * data.Stride];
                Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
                int xMin = int.MaxValue;
                int xMax = 0;
                int yMin = int.MaxValue;
                int yMax = 0;
                for (int y = 0; y < data.Height; y++)
                {
                    for (int x = 0; x < data.Width; x++)
                    {
                        byte alpha = buffer[y * data.Stride + 4 * x + 3];
                        if (alpha != 0)
                        {
                            if (x < xMin) xMin = x;
                            if (x > xMax) xMax = x;
                            if (y < yMin) yMin = y;
                            if (y > yMax) yMax = y;
                        }
                    }
                }
                if (xMax < xMin || yMax < yMin)
                {
                    // Image is empty...
                    return null;
                }
                srcRect = System.Drawing.Rectangle.FromLTRB(xMin, yMin, xMax, yMax);
            }
            finally
            {
                if (data != null)
                    source.UnlockBits(data);
            }

            Bitmap dest = new Bitmap(srcRect.Width, srcRect.Height);
            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(0, 0, srcRect.Width, srcRect.Height);
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(dest))
            {
                graphics.DrawImage(source, destRect, srcRect, GraphicsUnit.Pixel);
            }
            return dest;
        }



        private static byte[] HashDirectory(string directory)
        {
            List<string> files = Directory.GetFiles(directory, "*.png", SearchOption.AllDirectories).OrderBy(p => p).ToList();

            MD5 md5 = MD5.Create();

            for (int i = 0; i < files.Count; i++)
            {
                string file = files[i];

                string relativePath = file.Substring(directory.Length + 1);
                byte[] pathBytes = Encoding.UTF8.GetBytes(relativePath.ToLower());
                md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

                byte[] contentBytes = File.ReadAllBytes(file);
                if (i == files.Count - 1)
                    md5.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
                else
                    md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
            }

            return md5.Hash;


        }

        internal class TexturePackerException : Exception
        {
            internal TexturePackerException(string message):  base("TexturePackerException: " + message)
            {

            }
        }
    }
}
