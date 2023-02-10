using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightVx.Tests.Files
{
    internal class FileProvider
    {
        public static Stream GetFile(TestFileEnum file)
        {
            string name;
            switch (file)
            {
                case TestFileEnum.SmallGif:
                    name = "file_example_GIF_500kB.gif";
                    break;
                case TestFileEnum.SmallJpg:
                    name = "file_example_JPG_100kB.jpg";
                    break;
                case TestFileEnum.SmallPng:
                    name = "file_example_PNG_500kB.png";
                    break;
                case TestFileEnum.SmallSvg:
                    name = "file_example_SVG_20kB.svg";
                    break;
                default:
                    throw new NotImplementedException("unknown file");
            }
            string path = $"LightVx.Tests.Files.{name}";
            return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(FileProvider), name);
        }

    }
}
