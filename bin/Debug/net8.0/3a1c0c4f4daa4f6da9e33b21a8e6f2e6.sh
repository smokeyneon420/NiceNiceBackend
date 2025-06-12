function list_child_processes () {
    local ppid=$1;
    local current_children=$(pgrep -P $ppid);
    local local_child;
    if [ $? -eq 0 ];
    then
        for current_child in $current_children
        do
          local_child=$current_child;
          list_child_processes $local_child;
          echo $local_child;
        done;
    else
      return 0;
    fi;
}

ps 26467;
while [ $? -eq 0 ];
do
  sleep 1;
  ps 26467 > /dev/null;
done;

for child in $(list_child_processes 26469);
do
  echo killing $child;
  kill -s KILL $child;
done;
rm /Users/neothabethe/Documents/nicenice/nicenice.Server/bin/Debug/net8.0/3a1c0c4f4daa4f6da9e33b21a8e6f2e6.sh;
