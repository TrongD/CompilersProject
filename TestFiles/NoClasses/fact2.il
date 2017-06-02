//Generated Code in CIL
.assembly extern mscorlib {} 
.assembly fact2 {}
.method public static int32 factorial (int32)
{
.maxstack 100
ldarg.0
ldc.i4 0
beq L_01
ldarg.0
ldarg.0
ldc.i4 1
sub
call int32 factorial(int32)
mul
br Exit1
L_01:
ldc.i4 1
Exit1: 

ret
}
.method public static void main ()
{
.entrypoint
.maxstack 100
.locals init (int32, int32)
ldc.i4 0
stloc 0
ldstr "TCCL recursive factorial test"
call void [mscorlib]System.Console::WriteLine(string)
Expr1:
ldloc 0
ldc.i4 15
blt Body2
ret
Body2:
ldloc 0
call int32 factorial(int32)
stloc 1
ldstr "{0}"
ldloc 1
box int32
call void [mscorlib]System.Console::WriteLine(string, object) 
ldloc 0
ldc.i4 1
add
stloc 0
br Expr1

ret
}
