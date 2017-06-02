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
blt LessThan1
ldloc 1
stloc 0
br Exit1
LessThan1:
ldstr "In then part"
call void [mscorlib]System.Console::WriteLine(string)
Exit1: 
ldloc 0
ldc.i4 10
beq Equal2
ldstr "In else part"
call void [mscorlib]System.Console::WriteLine(string)
br Exit2
Equal2:
ldloc 0
stloc 1
Exit2: 
ldstr "{0}"
ldloc 1
box int32
call void [mscorlib]System.Console::WriteLine(string, object) 

ret
}
