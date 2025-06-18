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

ps 53149;
while [ $? -eq 0 ];
do
  sleep 1;
  ps 53149 > /dev/null;
done;

for child in $(list_child_processes 53153);
do
  echo killing $child;
  kill -s KILL $child;
done;
rm /Users/neothabethe/Documents/nicenice/nicenice.Server/bin/Debug/net8.0/f9c5c9647e1d442d9eede0bb81efc212.sh;
