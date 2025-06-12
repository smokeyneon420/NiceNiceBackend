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

ps 86519;
while [ $? -eq 0 ];
do
  sleep 1;
  ps 86519 > /dev/null;
done;

for child in $(list_child_processes 86522);
do
  echo killing $child;
  kill -s KILL $child;
done;
rm /Users/neothabethe/Documents/nicenice/nicenice.Server/bin/Debug/net8.0/0c3b9ef0ef0a4d3fbb6233d9693e883e.sh;
