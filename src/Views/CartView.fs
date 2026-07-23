// HITO 6 — Tab Pedido: líneas editables, total y envío a WhatsApp
module DonPizzero.Views.CartView

open Feliz
open DonPizzero
open DonPizzero.Types
open DonPizzero.State

let private soles (m: decimal) = sprintf "S/ %.2f" (float m)

let private lineaView (l: CartLine) (dispatch: Msg -> unit) =
    Html.div [
        prop.key (string l.Id)
        prop.className "rounded-2xl border border-carbon/5 bg-white p-4 shadow-sm"
        prop.children [
            Html.div [
                prop.className "flex items-start justify-between gap-2"
                prop.children [
                    Html.div [
                        prop.className "flex items-center gap-2"
                        prop.children [
                            Html.span [ prop.className "text-2xl"; prop.text l.Item.Emoji ]
                            Html.div [
                                prop.children [
                                    Html.h3 [
                                        prop.className "font-extrabold"
                                        prop.text (
                                            match l.Tamano with
                                            | Some t -> sprintf "%s (%s)" l.Item.Nombre (Size.etiqueta t)
                                            | None -> l.Item.Nombre)
                                    ]
                                    if not (List.isEmpty l.Extras) then
                                        Html.p [
                                            prop.className "text-xs text-carbon/60"
                                            prop.text (
                                                l.Extras
                                                |> List.map (fun e -> e.Nombre)
                                                |> String.concat ", "
                                                |> sprintf "+ %s")
                                        ]
                                ]
                            ]
                        ]
                    ]
                    Html.button [
                        prop.className "text-sm font-bold text-rosso/70 active:text-rosso"
                        prop.text "Quitar"
                        prop.onClick (fun _ -> dispatch (QuitarLinea l.Id))
                    ]
                ]
            ]
            Html.div [
                prop.className "mt-3 flex items-center justify-between"
                prop.children [
                    Html.div [
                        prop.className "flex items-center gap-3 rounded-lg border border-carbon/15 px-2 py-1"
                        prop.children [
                            Html.button [
                                prop.className "px-1.5 text-lg font-bold text-rosso"
                                prop.text "−"
                                prop.onClick (fun _ -> dispatch (CambiarCantidadLinea (l.Id, -1)))
                            ]
                            Html.span [
                                prop.className "w-5 text-center text-sm font-extrabold"
                                prop.text (string l.Cantidad)
                            ]
                            Html.button [
                                prop.className "px-1.5 text-lg font-bold text-rosso"
                                prop.text "+"
                                prop.onClick (fun _ -> dispatch (CambiarCantidadLinea (l.Id, 1)))
                            ]
                        ]
                    ]
                    Html.span [
                        prop.className "font-extrabold text-rosso"
                        prop.text (soles (CartLine.precioLinea l))
                    ]
                ]
            ]
        ]
    ]

let view (carrito: CartLine list) (dispatch: Msg -> unit) =
    Html.div [
        prop.className "flex flex-col"
        prop.children [
            Html.div [
                prop.className "px-4 pt-4"
                prop.children [
                    Html.div [
                        prop.className "w-fit rounded-full bg-white px-4 py-2 shadow-sm"
                        prop.children [
                            Html.p [
                                prop.className "text-[10px] font-extrabold uppercase tracking-widest text-carbon/50"
                                prop.text "Tu"
                            ]
                            Html.p [
                                prop.className "font-display text-lg leading-none text-carbon"
                                prop.text "Pedido"
                            ]
                        ]
                    ]
                ]
            ]

            if List.isEmpty carrito then
                Html.div [
                    prop.className "flex flex-col items-center justify-center gap-3 px-8 py-24 text-center"
                    prop.children [
                        Html.span [ prop.className "text-5xl"; prop.text "🛒" ]
                        Html.p [ prop.className "text-lg font-extrabold"; prop.text "Tu pedido está vacío" ]
                        Html.p [
                            prop.className "text-sm text-carbon/60"
                            prop.text "Agrega una pizza y empieza el antojo"
                        ]
                        Html.button [
                            prop.className "mt-2 rounded-xl bg-rosso px-8 py-3 font-extrabold text-white transition active:bg-rosso-dark"
                            prop.text "Ver la carta"
                            prop.onClick (fun _ -> dispatch (IrATab TabMenu))
                        ]
                    ]
                ]
            else
                Html.div [
                    prop.className "flex flex-col gap-3 p-4 pb-56"
                    prop.children [ for l in carrito -> lineaView l dispatch ]
                ]

            if not (List.isEmpty carrito) then
                Html.div [
                    prop.className "fixed inset-x-0 bottom-16 z-20 border-t border-carbon/10 bg-white p-4 shadow-2xl"
                    prop.children [
                        Html.div [
                            prop.className "mb-2 flex items-center justify-between"
                            prop.children [
                                Html.span [ prop.className "font-bold text-carbon/70"; prop.text "Total" ]
                                Html.span [
                                    prop.className "text-2xl font-extrabold text-rosso"
                                    prop.text (soles (Cart.total carrito))
                                ]
                            ]
                        ]
                        Html.p [
                            prop.className "mb-2 text-center text-xs font-bold text-verde"
                            prop.text (sprintf "🛵 Delivery GRATIS en %s" Data.negocio.Ciudad)
                        ]
                        Html.button [
                            prop.className "w-full rounded-xl bg-verde py-3.5 font-extrabold text-white transition active:bg-verde-dark"
                            prop.text "Enviar pedido por WhatsApp"
                            prop.onClick (fun _ -> dispatch EnviarPorWhatsApp)
                        ]
                    ]
                ]
        ]
    ]
