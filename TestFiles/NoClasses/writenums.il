//Generated Code in CIL
.assembly extern mscorlib {} 
.assembly writenums {}
.method public void main ()
{
.entrypoint
.maxstack 100
.locals init (int32, int32)
ldstr "{0}"
ldc.i4 5
box int32
call void [mscorlib]System.Console::WriteLine(string, object) 
ldstr "{0}"
ldc.i4 5
ldc.i4 2
mul
ldc.i4 6
ldc.i4 4
div
add
stloc 0
ldloc 0
box int32
call void [mscorlib]System.Console::WriteLine(string, object) 

ret
}
