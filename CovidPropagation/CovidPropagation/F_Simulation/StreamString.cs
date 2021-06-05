/*
 * Nom du projet : CovidPropagation
 * Auteur        : Joey Martig
 * Date          : 11.06.2021
 * Version       : 1.0
 * Description   : Simule la propagation du covid dans un environnement vaste représentant une ville.
 */
using System.Collections.Generic;
using System.Diagnostics;
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

        /// <summary>
        /// ID Documentation : Write_String
        /// Écrit en byte le message fournit en string, insère sa taille dans les 4 premiers bytes et envoies les données.
        /// </summary>
        /// <param name="outString"></param>
        public async void WriteString(string outString)
        {
            await Task.Run(() => {
                byte[] outBuffer = streamEncoding.GetBytes(outString);
                int len = outBuffer.Length;

                List<byte> dataToSend = new List<byte>();
                dataToSend.Add((byte)(len >> 24));
                dataToSend.Add((byte)(len >> 16));
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
