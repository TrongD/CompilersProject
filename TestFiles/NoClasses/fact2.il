//Generated Code in CIL
.assembly extern mscorlib {} 
.assembly fact2 {}
.method public static int32 factorial (int32)
{
.maxstack 100
.locals init (int32, int32)
ldarg.0

ret
}
.method public static void main ()
{
.entrypoint
.maxstack 100
.locals init (int32, int32)
.locals init (int32, int32, int32, int32)
ldc.i4 0
stloc 0
ldstr "TCCL recursive factorial test"
call void [mscorlib]System.Console::WriteLine(string)

ret
}
