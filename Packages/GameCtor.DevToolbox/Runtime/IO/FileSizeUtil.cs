using System;

namespace GameCtor.DevToolbox
{
    public static class FileSizeUtil
    {
        public static string Format(long fileSizeInBytes)
        {
            const long OneKiloByte = 1024;
            const long OneMegaByte = OneKiloByte * 1024;
            const long OneGigaByte = OneMegaByte * 1024;

            if (fileSizeInBytes < OneKiloByte)
            {
                return $"{fileSizeInBytes} bytes";
            }
            else if (fileSizeInBytes < OneMegaByte)
            {
                double kbSize = (double)fileSizeInBytes / OneKiloByte;
                return $"{kbSize:F2} KB";
            }
            else if (fileSizeInBytes < OneGigaByte)
            {
                double mbSize = (double)fileSizeInBytes / OneMegaByte;
                return $"{mbSize:F2} MB";
            }
            else
            {
                double gbSize = (double)fileSizeInBytes / OneGigaByte;
                return $"{gbSize:F2} GB";
            }
        }
    }
}
