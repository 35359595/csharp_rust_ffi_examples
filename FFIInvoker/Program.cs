using System;
using System.Text;
using System.Runtime.InteropServices;

namespace FFIInvoker
{
    internal class Native
    {
        const string ProvideLibPath = @"ffi_provider";

        [DllImport(ProvideLibPath, EntryPoint = "double_num", ExactSpelling = true)]
        public static extern int double_num(int n);

        [DllImport(ProvideLibPath, EntryPoint = "get_string", ExactSpelling = true)]
        public static extern StringHandle get_string();

        [DllImport(ProvideLibPath, EntryPoint = "to_lower", ExactSpelling = true)]
        public static extern unsafe StringHandle to_lower(IntPtr s);

        [DllImport(ProvideLibPath, EntryPoint = "string_free", ExactSpelling = true)]
        public static extern void string_free(IntPtr s);
    }

    internal class StringHandle : SafeHandle
    {
        public StringHandle() : base(IntPtr.Zero, true) { }

        public override bool IsInvalid
        {
            get { return handle == IntPtr.Zero; }
        }

        protected override bool ReleaseHandle()
        {
            if (!IsInvalid)
                Native.string_free(handle);
            return true;
        }

        public string IntoString() =>
            Marshal.PtrToStringUTF8(handle) ?? "Failed to process";
    }

    class Program
    {

        static void Main(string[] args)
        {
            var res = Native.double_num(2);
            Console.WriteLine($"2X2={res}");

            StringHandle HelloString = Native.get_string();
            Console.WriteLine(HelloString.IntoString());
            HelloString.Dispose();

            StringHandle LowerString = Native.to_lower(Marshal.StringToCoTaskMemUTF8("BooM"));
            Console.WriteLine(LowerString.IntoString());
            LowerString.Dispose();
        }
    }
}
