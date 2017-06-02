//Generated Code in CIL
.assembly extern mscorlib {} 
.assembly iftest {}
.method public static void main ()
{
.entrypoint
.maxstack 100
.locals init (int32, int32)
ldc.i4 0
stloc 0
ldc.i4 1
stloc 1
ldstr "TCCL if test"
call void [mscorlib]System.Console::WriteLine(string)
ldloc 0
ldc.i4 10
blt L_03
ldloc 1
stloc 0
br Exit3
L_03:
ldstr "In then part"
call void [mscorlib]System.Console::WriteLine(string)
Exit3: 
ldloc 0
ldc.i4 10
beq L_04
ldstr "In else part"
call void [mscorlib]System.Console::WriteLine(string)
br Exit4
L_04:
ldloc 0
stloc 1
Exit4: 

ret
}
