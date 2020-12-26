namespace RaytrAkkar.Lisp
open Parser
open LispTypes
open Evaluator
open ResultMonad

module GetScene =
    let SimpleScene (src: string) : RaytrAkkar.Raytracer.SimpleScene =
        let res = result {
            let! parsed = parser src
            let! e = evaluator parsed
            let! returnObject = match e with
                | (LispWrapper obj, _) -> match obj with
                                     | :? RaytrAkkar.Raytracer.SimpleScene as typed -> Ok typed
                | _ -> Error "Evaluation did not return a SimpleScene"

            return returnObject
            }
        let returnObject =
            match res with
                | Ok v -> v
                | Error msg -> failwith msg

        returnObject
