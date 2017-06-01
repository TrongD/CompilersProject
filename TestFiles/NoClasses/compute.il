//Generated Code in CIL
.assembly extern mscorlib {} 
.assembly compute {}
.method public void main ()
{
.entrypoint
.maxstack 100
.locals init (int32, int32, int32, int32)
ldc.i4 3
ldc.i4 4
add
stloc 1
ldstr "Value of x = "
call void [mscorlib]System.Console::Write(string)
ldstr "{0}"
ldloc 1
box int32
call void [mscorlib]System.Console::WriteLine(string, object) 
ldloc 1
ldc.i4 5
sub
stloc 0
ldstr "Value of w = "
call void [mscorlib]System.Console::Write(string)
ldstr "{0}"
ldloc 0
box int32
call void [mscorlib]System.Console::WriteLine(string, object) 
ldstr "value of big expression ="
call void [mscorlib]System.Console::Write(string)
ldstr "{0}"
ldloc 0
ldloc 1
mul
ldc.i4 12
ldloc 0
div
add
stloc 2
ldloc 2
box int32
call void [mscorlib]System.Console::WriteLine(string, object) 

ret
}
