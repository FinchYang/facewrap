for i in `cat $1 `
do 
rm  "$2/$i"_?.jpg
done
