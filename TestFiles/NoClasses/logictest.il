//Generated Code in CIL
.assembly extern mscorlib {} 
.assembly logictest {}
.method public static void outStrln (string)
{
.maxstack 100
ldarg.0
call void [mscorlib]System.Console::WriteLine(string)

ret
}
.method public static void main ()
{
.entrypoint
.maxstack 100
.locals init (int32, int32)
ldc.i4 0
stloc 0
ldc.i4 1
stloc 1
ldstr "TCCL logic test"
call void outStrln(string)
ldloc 0
ldc.i4 10
blt L_01
ldloc 1
stloc 0
br Exit1
L_01:
ldstr "In then part of & test"
call void outStrln(string)
Exit1: 
ldloc 0
ldc.i4 10
blt L_02
L_02:
ldloc 0
L_03
ldstr "In else part of 2nd & test"
call void outStrln(string)
br Exit3
L_03:
ldstr "In then part of 2nd & test erroneously"
call void outStrln(string)
Exit3: 
ldloc 0
ldc.i4 10
beq ldloc 1
ldloc 0
beq L_04
ldstr "In else part of false | test"
call void outStrln(string)
br Exit4
L_04:
ldloc 0
stloc 1
Exit4: 
ldloc 0
ldc.i4 1
beq ldloc 1
ldc.i4 1
beq L_05
ldstr "In else part of true | test erroneously"
call void outStrln(string)
br Exit5
L_05:
ldstr "In then part of true | test"
call void outStrln(string)
Exit5: 

ret
}
