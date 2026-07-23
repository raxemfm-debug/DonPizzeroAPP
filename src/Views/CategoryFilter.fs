// HITO 6 — Chips de categorías con emoji y scroll horizontal
module DonPizzero.Views.CategoryFilter

open Feliz
open DonPizzero.Types
open DonPizzero.State

let private chip (texto: string) (activa: bool) (alPulsar: unit -> unit) =
    Html.button [
        prop.className (
            "shrink-0 rounded-full px-4 py-1.5 text-sm font-bold transition active:scale-95 "
            + (if activa then "bg-rosso text-white shadow"
               else "border border-carbon/15 bg-white text-carbon"))
        prop.onClick (fun _ -> alPulsar ())
        prop.text texto
    ]

let view (categoriaActiva: Category option) (dispatch: Msg -> unit) =
    Html.nav [
        prop.className "no-scrollbar flex gap-2 overflow-x-auto px-4 py-3"
        prop.children [
            chip "Todo" categoriaActiva.IsNone (fun () ->
                dispatch (SeleccionarCategoria None))
            for cat in Category.todas do
                chip
                    (sprintf "%s %s" (Category.emoji cat) (Category.etiqueta cat))
                    (categoriaActiva = Some cat)
                    (fun () -> dispatch (SeleccionarCategoria (Some cat)))
        ]
    ]
