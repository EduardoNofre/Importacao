using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TCIReleaseJucec.model;

namespace TCIReleaseJucec.Util
{
    class LogUtil
    {
        public void escreveLog(string msg, ConfiguracaoProperties config)
        {

            try
            {

                if (!string.IsNullOrEmpty(config.caminhoLog))
                {
                    if (!Directory.Exists(config.caminhoLog + "\\log\\"))
                    {
                        Directory.CreateDirectory(config.caminhoLog);
                    }
                }

                using (StreamWriter streamWriter = new StreamWriter(config.caminhoLog + "log" + DateTime.Now.ToString("yyyyMMdd_HH") + "00.txt", true, Encoding.Default))
                {
                    streamWriter.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + msg);
                    streamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }
    }
}
