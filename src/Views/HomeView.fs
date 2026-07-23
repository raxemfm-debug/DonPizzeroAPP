// HITO 6.1 — Tab Inicio: hero "sobrino", buscador, selector de local y populares
module DonPizzero.Views.HomeView

open Feliz
open DonPizzero
open DonPizzero.Types
open DonPizzero.State

let private soles (m: decimal) = sprintf "S/ %.2f" (float m)

let private tarjetaPopular (item: MenuItem) (dispatch: Msg -> unit) =
    Html.button [
        prop.key item.Id
        prop.className "w-36 shrink-0 rounded-2xl border border-carbon/5 bg-white p-3 text-left shadow-sm transition active:scale-95"
        prop.onClick (fun _ -> dispatch (AbrirModal item))
        prop.children [
            Html.div [
                prop.className "flex h-20 items-center justify-center rounded-xl bg-crema text-4xl"
                prop.text item.Emoji
            ]
            Html.p [ prop.className "mt-2 truncate text-sm font-extrabold"; prop.text item.Nombre ]
            Html.p [
                prop.className "text-sm font-extrabold text-rosso"
                prop.text (sprintf "desde %s" (soles (Pricing.desde item.Precio)))
            ]
        ]
    ]

let view (model: Model) (dispatch: Msg -> unit) =
    Html.div [
        prop.className "flex flex-col gap-4 px-4 pb-28 pt-4"
        prop.children [
            // fila superior: selector de local de entrega
            Html.div [
                prop.className "flex items-center justify-between"
                prop.children [
                    Html.button [
                        prop.className "rounded-full bg-white px-4 py-2 text-left shadow-sm transition active:scale-95"
                        prop.onClick (fun _ -> dispatch (AbrirSelectorLocal ParaEntrega))
                        prop.children [
                            Html.p [
                                prop.className "text-[10px] font-extrabold uppercase tracking-widest text-carbon/50"
                                prop.text "Entregar en"
                            ]
                            Html.p [
                                prop.className "text-sm font-extrabold leading-none text-carbon"
                                prop.text (
                                    (model.LocalEntrega |> Option.defaultValue "Elige tu local") + " ▾")
                            ]
                        ]
                    ]
                    Html.div [
                        prop.className "rounded-full bg-white p-3 shadow-sm"
                        prop.children [ Html.span [ prop.className "text-xl"; prop.text "🔔" ] ]
                    ]
                ]
            ]

            // hero verde
            Html.div [
                prop.className "relative overflow-hidden rounded-3xl bg-verde p-5 text-white"
                prop.children [
                    Html.span [
                        prop.className "inline-block rounded-full bg-dorado px-3 py-1 text-xs font-extrabold text-carbon"
                        prop.text "🛵 DELIVERY GRATIS"
                    ]
                    Html.h2 [
                        prop.className "mt-3 font-display text-3xl leading-tight"
                        prop.text "¡Hola, sobrino!"
                    ]
                    Html.h3 [
                        prop.className "font-display text-2xl leading-tight text-white/95"
                        prop.text "Tu pizza a un toque"
                    ]
                    Html.p [
                        prop.className "mt-1 text-sm font-bold text-white/85"
                        prop.text "Pide por WhatsApp y recíbela en tu puerta"
                    ]
                    Html.span [
                        prop.className "absolute -bottom-3 -right-2 rotate-12 text-7xl"
                        prop.text "🍕"
                    ]
                ]
            ]

            // buscador funcional
            Html.div [
                prop.className "flex items-center gap-2 rounded-2xl border border-carbon/10 bg-white px-4 py-3 shadow-sm"
                prop.children [
                    Html.span [ prop.text "🔍" ]
                    Html.input [
                        prop.className "flex-1 bg-transparent text-sm font-bold outline-none placeholder:font-normal"
                        prop.type' "text"
                        prop.placeholder "¿Qué se te antoja hoy?"
                        prop.value model.Busqueda
                        prop.onChange (fun (v: string) -> dispatch (Buscar v))
                    ]
                ]
            ]

            if model.Busqueda.Trim() <> "" then
                let resultados = resultadosBusqueda model
                Html.div [
                    prop.children [
                        Html.h3 [
                            prop.className "mb-2 text-lg font-extrabold"
                            prop.text (sprintf "Resultados (%d)" (List.length resultados))
                        ]
                        if List.isEmpty resultados then
                            Html.p [
                                prop.className "text-sm text-carbon/60"
                                prop.text "Nada por aquí… prueba con \"pizza\", \"criolla\" o \"pasta\"."
                            ]
                        else
                            Html.div [
                                prop.className "-mx-4"
                                prop.children [ MenuList.view resultados dispatch ]
                            ]
                    ]
                ]
            else
                Html.div [
                    prop.className "flex flex-col gap-4"
                    prop.children [
                        // accesos rápidos
                        Html.div [
                            prop.className "grid grid-cols-2 gap-3"
                            prop.children [
                                Html.button [
                                    prop.className "flex items-center justify-center gap-2 rounded-2xl border border-carbon/5 bg-white py-3 font-extrabold shadow-sm transition active:scale-95"
                                    prop.text "🛵 Pedir ahora"
                                    prop.onClick (fun _ -> dispatch (IrATab TabMenu))
                                ]
                                Html.button [
                                    prop.className "flex items-center justify-center gap-2 rounded-2xl border border-carbon/5 bg-white py-3 font-extrabold shadow-sm transition active:scale-95"
                                    prop.text "🍽️ Reservar mesa"
                                    prop.onClick (fun _ -> dispatch (AbrirSelectorLocal ParaReserva))
                                ]
                            ]
                        ]
                        // las más pedidas
                        Html.div [
                            prop.className "flex items-center justify-between"
                            prop.children [
                                Html.h3 [ prop.className "text-lg font-extrabold"; prop.text "Las más pedidas" ]
                                Html.button [
                                    prop.className "text-sm font-bold text-rosso"
                                    prop.text "Ver todo"
                                    prop.onClick (fun _ -> dispatch (IrATab TabMenu))
                                ]
                            ]
                        ]
                        Html.div [
                            prop.className "no-scrollbar -mx-4 flex gap-3 overflow-x-auto px-4"
                            prop.children [
                                for item in Data.menu |> List.filter (fun i -> i.Popular) ->
                                    tarjetaPopular item dispatch
                            ]
                        ]
                    ]
                ]
        ]
    ]
