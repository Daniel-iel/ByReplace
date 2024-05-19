restore:
	dotnet restore

build:
	dotnet build -c Release

test:
	dotnet test --no-build --verbosity normal /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

test-nocoverage:
	dotnet test --no-build --verbosity normal /p:CollectCoverage=false

test-mutant:
	cd ByReplace.Test && dotnet stryker --reporter "html"

test-docker:
	docker build -t byreplace-docker-test . && docker run byreplace-docker-test

format:
	dotnet format '.\' -v diag --severity error --verbosity --no-restore

dependense-diagram:
	dependensee -S "src"  -O  "src/diagram.html"

analyze-package-usage:
	snitch

run-ci:
	act "on: pull_request"  -C ../