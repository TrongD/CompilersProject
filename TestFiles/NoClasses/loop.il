//Generated Code in CIL
.assembly extern mscorlib {} 
.assembly loop {}
.method public static void main ()
{
.entrypoint
.maxstack 100
.locals init (int32, int32)
ldc.i4 0
stloc 0
ldc.i4 1
stloc 1
ldstr "Beginning loop test"
call void [mscorlib]System.Console::WriteLine(string)
Expr1:
ldloc 0
ldc.i4 10
blt Body2
ret
Body2:
ldstr "{0}"
ldloc 1
box int32
call void [mscorlib]System.Console::WriteLine(string, object) 
ldloc 0
ldc.i4 1
add
stloc 0
ldloc 1
ldloc 0
mul
stloc 1
br Expr1

ret
}
