#!/bin/sh
dotnet run ./test/code/test-mini.tri > ./test_output;
diff ./test_output ./test/expected/test-mini-output.txt;
rm ./test_output;
