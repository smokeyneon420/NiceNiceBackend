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

ps 81313;
while [ $? -eq 0 ];
do
  sleep 1;
  ps 81313 > /dev/null;
done;

for child in $(list_child_processes 81321);
do
  echo killing $child;
  kill -s KILL $child;
done;
rm /Users/neothabethe/Documents/nicenice/nicenice.Server/bin/Debug/net8.0/b73e6ef4c6764b1d87f2acd46afc4da5.sh;
