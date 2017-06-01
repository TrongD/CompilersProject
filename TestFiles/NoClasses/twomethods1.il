//Generated Code in CIL
.assembly extern mscorlib {} 
.assembly twomethods1 {}
.method public void check (int32)
{
.maxstack 100
.locals init (int32, int32)
ldstr "The argument to check was "
call void [mscorlib]System.Console::Write(string)
ldstr "{0}"
ldarg.0
box int32
call void [mscorlib]System.Console::WriteLine(string, object) 

ret
}
.method public void main ()
{
.entrypoint
.maxstack 100
.locals init (int32, int32)
ldc.i4 12345
call void check(int32)
ldstr "Back in main"
call void [mscorlib]System.Console::WriteLine(string)

ret
}
