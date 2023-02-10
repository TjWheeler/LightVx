using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LightVx.Validators.File
{
    public abstract class SignatureValidator : ValidatorBase
    {
        public SignatureValidator(string name, byte[] startBytes = null, byte[] endBytes = null)
        {
            StartBytes = startBytes;
            EndBytes = endBytes;
            Name = name;
        }

        public byte[] StartBytes { get; protected set; }
        public byte[] EndBytes { get; protected set; }
        public string Name { get; set; }
        protected virtual bool MatchBytes(byte[] bytes, Stream stream)
        {
            var buffer = new byte[bytes.Length];
            int readCount = stream.Read(buffer, 0, bytes.Length);
            if (readCount != bytes.Length)
            {
                return false;
            }
            return bytes.SequenceEqual(buffer);
        }
        public override void Validate()
        {
            if (!(_Input is Stream))
            {
                Fail("must be a Stream");
                return;
            }

            if (StartBytes == null && EndBytes == null)
            {
                Fail("Invalid operation.  Cannot validate signature with null start and end bytes");
                return;
            }
            Stream inputStream = (Stream)_Input;
            

            if (StartBytes != null)
            {
                if (inputStream.CanSeek)
                {
                    inputStream.Seek(0, SeekOrigin.Begin);
                }
                if (!MatchBytes(StartBytes, inputStream))
                {
                    Fail($"is not a valid {Name}");
                    return;
                }
            }

            if (EndBytes != null)
            {
                if (!inputStream.CanSeek)
                {
                    Fail("is not a seekable stream and cannot be validated");
                    return;
                }
                inputStream.Seek(-EndBytes.Length, SeekOrigin.End);
                if (!MatchBytes(EndBytes, inputStream))
                {
                    Fail($"is not a valid {Name}");
                    return;
                }
            }
            Succeed();
        }
    }
}
