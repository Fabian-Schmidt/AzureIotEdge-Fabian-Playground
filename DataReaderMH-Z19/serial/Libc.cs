using System;
using System.Runtime.InteropServices;

namespace DataReaderMHZ19.serial
{
    public static class Libc
    {
        [Flags]
        public enum OpenFlags
        {
            O_RDONLY = 0,
            O_WRONLY = 1,
            O_RDWR = 2,

            O_NONBLOCK = 4,
        }

        /** from termios.h */
        [Flags]
        public enum c_cflag_bits : long
        {
            CSIZE = 0x0000060,
            CS8 = 0x0000060,
            CSTOPB = 0x0000100,
            PARENB = 0x0000400,

            CREAD = 0x0000200,
            CLOCAL = 0x0004000,

            CRTSCTS = 0x020000000000
        }


        [DllImport("libc.so.6")]
        public static extern int getpid();

        [DllImport("libc.so.6")]
        public static extern int tcgetattr(int fd, [Out] byte[] termios_data);

        [DllImport("libc.so.6")]
        public static extern int open(string pathname, OpenFlags flags);

        [DllImport("libc.so.6")]
        public static extern int close(int fd);

        [DllImport("libc.so.6")]
        public static extern int read(int fd, IntPtr buf, int count);

        [DllImport("libc.so.6")]
        public static extern int write(int fd, IntPtr buf, int count);

        [DllImport("libc.so.6")]
        public static extern int tcsetattr(int fd, int optional_actions, byte[] termios_data);

        [DllImport("libc.so.6")]
        public static extern int cfsetspeed(byte[] termios_data, BaudRate speed);
    }
}