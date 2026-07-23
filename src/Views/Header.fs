// HITO 6 — Cabecera del tab Menú: píldora de título, carrito y franja verde
module DonPizzero.Views.Header

open Feliz
open DonPizzero
open DonPizzero.Types
open DonPizzero.State

let view (carrito: CartLine list) (dispatch: Msg -> unit) =
    Html.header [
        prop.className "flex flex-col gap-3 px-4 pt-4"
        prop.children [
            Html.div [
                prop.className "flex items-center justify-between"
                prop.children [
                    Html.div [
                        prop.className "rounded-full bg-white px-4 py-2 shadow-sm"
                        prop.children [
                            Html.p [
                                prop.className "text-[10px] font-extrabold uppercase tracking-widest text-carbon/50"
                                prop.text "Nuestra"
                            ]
                            Html.p [
                                prop.className "font-display text-lg leading-none text-carbon"
                                prop.text "Carta Pizzera"
                            ]
                        ]
                    ]
                    Html.button [
                        prop.className "relative rounded-full bg-white p-3 shadow-sm transition active:scale-95"
                        prop.ariaLabel "Ver pedido"
                        prop.onClick (fun _ -> dispatch (IrATab TabPedido))
                        prop.children [
                            Html.span [ prop.className "text-xl"; prop.text "🛒" ]
                            if not (List.isEmpty carrito) then
                                Html.span [
                                    prop.className "absolute -right-1 -top-1 flex h-5 w-5 items-center justify-center rounded-full bg-rosso text-xs font-bold text-white"
                                    prop.text (string (Cart.cantidadItems carrito))
                                ]
                        ]
                    ]
                ]
            ]
            Html.div [
                prop.className "flex items-center justify-between rounded-xl bg-verde px-4 py-2 text-xs font-bold text-white"
                prop.children [
                    Html.span [ prop.text "🛵 DELIVERY GRATIS" ]
                    match List.tryHead Data.negocio.Horarios with
                    | Some h ->
                        Html.span [ prop.text (sprintf "%s · %s - %s" h.Dias h.Apertura h.Cierre) ]
                    | None -> Html.none
                ]
            ]
        ]
    ]
