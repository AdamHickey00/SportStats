// Include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing.XUnit2

// Directories
let buildDir  = "./build"
let appReferences = !! "*.sln"

// Targets
let clean _ = CleanDirs [buildDir]

let install () =
    let execParams : Fake.ProcessHelper.ExecParams = {
        Program = "./.paket/paket.exe"
        CommandLine = "install"
        WorkingDirectory = "./"
        Args = []
    }

    let result = Fake.ProcessHelper.shellExec execParams

    if result <> 0 then
        printfn "Error installing packages"
    else
        printfn "Packages successfully installed"

let build _ = MSBuildRelease buildDir "Build" appReferences |> Log "Build: "
let test () =
    !! (buildDir + "/*Tests.dll")
    |> xUnit2 (fun p -> {p with NoAppDomain = true})

Target "Clean" clean
Target "Install" install
Target "Build" build
Target "Test" test

// Build order
"Clean"
==> "Install"
==> "Build"

// Test
"Clean"
==> "Install"
==> "Build"
==> "Test"

RunTargetOrDefault "Build"
