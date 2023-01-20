nasm -f win32 m.asm
gcc -m32 -mconsole m.obj -o m
move ./m.asm ./m
move ./m.obj ./m
move ./m.exe ./m
