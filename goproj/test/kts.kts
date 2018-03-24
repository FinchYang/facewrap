import java.io.File
val folders=File(args[0]).listFiles{file1->file1.isDirectory()}
folders?.forEach{f->println(f)}