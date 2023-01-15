nasm -f win32 test.asm
gcc -m32 -mconsole test.obj -o test
move ./test.asm ./test
move ./test.obj ./test
move ./test.exe ./test
