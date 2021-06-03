/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidPropagation
{
    public class StreamString
    {
        private BinaryWriter stream;
        private UnicodeEncoding streamEncoding;

        public StreamString(Stream stream)
        {
            this.stream = new BinaryWriter(stream);
            streamEncoding = new UnicodeEncoding();
        }

        public async void WriteString(string outString)
        {
            await Task.Run(() => {
                byte[] outBuffer = streamEncoding.GetBytes(outString);
                int len = outBuffer.Length;

                List<byte> dataToSend = new List<byte>();
                dataToSend.Add((byte)(len >> 8));
                dataToSend.Add((byte)(len >> 0));
                dataToSend.AddRange(outBuffer.ToList());
                stream.Write(dataToSend.ToArray(), 0, dataToSend.Count);
                stream.Flush();
            });

        }

        public void CloseLink()
        {
            if (stream != null)
            {
                stream.Close();
            }

        }
    }
}
