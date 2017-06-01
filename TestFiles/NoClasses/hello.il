//Generated Code in CIL
.assembly extern mscorlib {} 
.assembly hello {}
.method public void main ()
{
.entrypoint
.maxstack 100
.locals init (int32, int32)
ldstr "Hello, World"
call void [mscorlib]System.Console::WriteLine(string)

ret
}
