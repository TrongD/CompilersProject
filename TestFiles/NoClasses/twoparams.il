//Generated Code in CIL
.assembly extern mscorlib {} 
.assembly twoparams {}
.method public static void outTwoInts (int32, int32)
{
.maxstack 100
.locals init (int32, int32)
ldstr "{0}"
ldarg.0
box int32
call void [mscorlib]System.Console::WriteLine(string, object) 
ldstr " is the value of the first parameter"
call void [mscorlib]System.Console::WriteLine(string)
ldstr "{0}"
ldarg.1
box int32
call void [mscorlib]System.Console::WriteLine(string, object) 
ldstr " is the value of the second parameter"
call void [mscorlib]System.Console::WriteLine(string)

ret
}
.method public static void main ()
{
.entrypoint
.maxstack 100
.locals init (int32, int32)
.locals init (int32, int32, int32, int32, int32)
ldstr "TCCL test of method with two parameters test"
call void [mscorlib]System.Console::WriteLine(string)
ldc.i4 3
ldc.i4 4
add
stloc 2
ldc.i4 5
ldc.i4 7
mul
stloc 1
ldloc 2
ldloc 1
call void outTwoInts(int32, int32)

ret
}
