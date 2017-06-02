//Generated Code in CIL
.assembly extern mscorlib {} 
.assembly twomethods0 {}
.method public static void check ()
{
.maxstack 100
ldstr "In the check method"
call void [mscorlib]System.Console::WriteLine(string)

ret
}
.method public static void main ()
{
.entrypoint
.maxstack 100
call void check()
ldstr "Back in main"
call void [mscorlib]System.Console::WriteLine(string)

ret
}
