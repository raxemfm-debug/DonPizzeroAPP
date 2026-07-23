// HITO 6 — Tarjetas del menú con icono emoji (tap → modal)
module DonPizzero.Views.MenuList

open Feliz
open DonPizzero.Types
open DonPizzero.State

let private soles (m: decimal) = sprintf "S/ %.2f" (float m)

let private tarjeta (item: MenuItem) (dispatch: Msg -> unit) =
    Html.button [
        prop.key item.Id
        prop.className "flex w-full items-center gap-3 rounded-2xl border border-carbon/5 bg-white p-4 text-left shadow-sm transition active:scale-[0.98]"
        prop.onClick (fun _ -> dispatch (AbrirModal item))
        prop.children [
            Html.div [
                prop.className "flex h-14 w-14 shrink-0 items-center justify-center rounded-xl bg-crema text-3xl"
                prop.text item.Emoji
            ]
            Html.div [
                prop.className "flex flex-1 flex-col gap-0.5"
                prop.children [
                    match item.Etiqueta with
                    | Some e ->
                        Html.span [
                            prop.className "w-fit rounded-full bg-dorado/15 px-2 py-0.5 text-[10px] font-extrabold uppercase tracking-wide text-dorado"
                            prop.text e
                        ]
                    | None -> Html.none
                    Html.h3 [ prop.className "font-extrabold"; prop.text item.Nombre ]
                    Html.p [ prop.className "text-xs text-carbon/60"; prop.text item.Descripcion ]
                    Html.p [
                        prop.className "mt-0.5 text-sm font-extrabold text-rosso"
                        prop.text (
                            match item.Precio with
                            | Fijo p -> soles p
                            | PorTamano _ -> sprintf "desde %s" (soles (Pricing.desde item.Precio)))
                    ]
                ]
            ]
            Html.span [
                prop.className "shrink-0 rounded-full bg-rosso px-3 py-1 text-xl font-bold text-white"
                prop.text "+"
            ]
        ]
    ]

let view (items: MenuItem list) (dispatch: Msg -> unit) =
    Html.main [
        prop.className "flex flex-col gap-3 px-4 pb-28"
        prop.children [ for item in items -> tarjeta item dispatch ]
    ]
