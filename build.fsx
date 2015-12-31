// Include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"

open Fake

// Directories
let buildDir  = "./build"
let appReferences = !! "*.fsproj"

// Targets
let clean _ = CleanDirs [buildDir]
let build _ = MSBuildRelease buildDir "Build" appReferences |> Log "Build: "

Target "Clean" clean
Target "Build" build

// Build order
"Clean"
==> "Build"

RunTargetOrDefault "Build"
