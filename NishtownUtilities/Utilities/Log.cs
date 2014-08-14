using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Nishtown.Utilities
{
    class Log
    {

        public string logfile { get; set; }
        public bool timestamp { get; set; }
        public bool rotate { get; set; }
        private int rotatedays { get; set; }

        public Log()
        {
            timestamp = false;
            rotatedays = 0;
        }

        public Log(string file, bool usetimestamp = false)
        {
            logfile = file;
            rotatedays = 0;
            timestamp = usetimestamp;
        }

        public void RotationPeriod(int numberofDays)
        {
            rotatedays = numberofDays;
        }

        public void Rotatelog()
        {
            if (File.Exists(logfile))
            {
                DateTime ft = File.GetLastWriteTime(logfile);

                if ((DateTime.Now.Date - ft.Date).TotalDays >= rotatedays)
                {
                    string sPath = Path.GetDirectoryName(logfile);
                    string sFile = String.Format("{0:yyyy-MM-dd}_{1}", ft.Date, Path.GetFileName(logfile));
                    int i = 1;
                    while (File.Exists(Path.Combine(sPath, sFile)))
                    {
                        sFile = String.Format("{0:yyyy-MM-dd}_{1}_{2}", ft.Date, Path.GetFileName(logfile), i);
                        i++;
                    }
                    File.Move(logfile, Path.Combine(sPath, sFile));
                }
            }
        }

        public void Write(string text)
        {
            if (logfile != null)
            {
                if (!Directory.Exists(Path.GetDirectoryName(logfile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(logfile));
                }
                if (rotate)
                {
                    this.Rotatelog();
                }
                using (StreamWriter sw = new StreamWriter(logfile, true))
                {
                    if (timestamp)
                    {
                        sw.WriteLine(DateTime.Now + ": " + text);
                    }
                    else
                    {
                        sw.WriteLine(text);
                    }
                }
            }
            else
            {
                throw new Exception("Log file not set");
            }

        }
    }
}
