// HITO 4 — Bottom-sheet de producto: tamaños, extras, cantidad y total dinámico
module DonPizzero.Views.ProductModal

open Feliz
open DonPizzero.Types
open DonPizzero.State

let private soles (m: decimal) = sprintf "S/ %.2f" (float m)

/// Línea temporal para reutilizar el cálculo de precios del dominio
let private lineaTemporal (m: ModalState) : CartLine =
    { Id = 0; Item = m.Item; Tamano = m.Tamano; Extras = m.Extras; Cantidad = m.Cantidad }

let private pillTamano (modal: ModalState) (precios: Map<Size, decimal>) (t: Size) (dispatch: Msg -> unit) =
    let activa = modal.Tamano = Some t
    Html.button [
        prop.className (
            "flex flex-1 flex-col items-center rounded-xl border-2 px-2 py-2 transition active:scale-95 "
            + (if activa then "border-rosso bg-rosso/5 text-rosso"
               else "border-carbon/10 bg-white text-carbon"))
        prop.onClick (fun _ -> dispatch (ElegirTamano t))
        prop.children [
            Html.span [ prop.className "text-sm font-extrabold"; prop.text (Size.etiqueta t) ]
            Html.span [
                prop.className "text-xs font-bold opacity-70"
                prop.text (precios |> Map.tryFind t |> Option.map soles |> Option.defaultValue "-")
            ]
        ]
    ]

let private filaExtra (modal: ModalState) (extra: Extra) (dispatch: Msg -> unit) =
    let marcado = modal.Extras |> List.exists (fun e -> e.Id = extra.Id)
    Html.button [
        prop.key extra.Id
        prop.className "flex w-full items-center justify-between rounded-xl border border-carbon/10 bg-white px-3 py-2.5 transition active:scale-[0.99]"
        prop.onClick (fun _ -> dispatch (AlternarExtra extra))
        prop.children [
            Html.div [
                prop.className "flex items-center gap-2"
                prop.children [
                    Html.span [
                        prop.className (
                            "flex h-5 w-5 items-center justify-center rounded-md border-2 text-xs font-bold "
                            + (if marcado then "border-verde bg-verde text-white"
                               else "border-carbon/25 text-transparent"))
                        prop.text "✓"
                    ]
                    Html.span [ prop.className "text-sm font-bold"; prop.text extra.Nombre ]
                ]
            ]
            Html.span [
                prop.className "text-sm font-bold text-carbon/60"
                prop.text (sprintf "+ %s" (soles extra.Precio))
            ]
        ]
    ]

let view (modal: ModalState) (dispatch: Msg -> unit) =
    Html.div [
        prop.className "fixed inset-0 z-40 flex items-end bg-carbon/50"
        prop.onClick (fun _ -> dispatch CerrarModal)
        prop.children [
            Html.div [
                prop.className "max-h-[85vh] w-full overflow-y-auto rounded-t-3xl bg-crema p-5 pb-8"
                prop.onClick (fun e -> e.stopPropagation ())
                prop.children [
                    Html.div [ prop.className "mx-auto mb-4 h-1.5 w-10 rounded-full bg-carbon/20" ]

                    Html.div [
                        prop.className "flex items-start justify-between gap-3"
                        prop.children [
                            Html.div [
                                prop.children [
                                    Html.h3 [
                                        prop.className "font-display text-2xl text-carbon"
                                        prop.text modal.Item.Nombre
                                    ]
                                    Html.p [
                                        prop.className "mt-1 text-sm text-carbon/60"
                                        prop.text modal.Item.Descripcion
                                    ]
                                ]
                            ]
                            Html.button [
                                prop.className "rounded-full bg-carbon/10 px-3 py-1 font-bold text-carbon/60 active:bg-carbon/20"
                                prop.ariaLabel "Cerrar"
                                prop.text "✕"
                                prop.onClick (fun _ -> dispatch CerrarModal)
                            ]
                        ]
                    ]

                    match modal.Item.Precio with
                    | PorTamano precios ->
                        Html.div [
                            prop.className "mt-4"
                            prop.children [
                                Html.h4 [
                                    prop.className "mb-2 text-sm font-extrabold uppercase tracking-wide text-carbon/60"
                                    prop.text "Elige el tamaño"
                                ]
                                Html.div [
                                    prop.className "flex gap-2"
                                    prop.children [ for t in Size.todos -> pillTamano modal precios t dispatch ]
                                ]
                            ]
                        ]
                    | Fijo _ -> Html.none

                    if not (List.isEmpty modal.Item.ExtrasDisponibles) then
                        Html.div [
                            prop.className "mt-4"
                            prop.children [
                                Html.h4 [
                                    prop.className "mb-2 text-sm font-extrabold uppercase tracking-wide text-carbon/60"
                                    prop.text "Extras"
                                ]
                                Html.div [
                                    prop.className "flex flex-col gap-2"
                                    prop.children [ for e in modal.Item.ExtrasDisponibles -> filaExtra modal e dispatch ]
                                ]
                            ]
                        ]

                    Html.div [
                        prop.className "mt-5 flex items-center gap-3"
                        prop.children [
                            Html.div [
                                prop.className "flex items-center gap-3 rounded-xl border border-carbon/15 bg-white px-2 py-1.5"
                                prop.children [
                                    Html.button [
                                        prop.className "px-2 text-xl font-bold text-rosso"
                                        prop.text "−"
                                        prop.onClick (fun _ -> dispatch (CambiarCantidadModal (-1)))
                                    ]
                                    Html.span [
                                        prop.className "w-6 text-center font-extrabold"
                                        prop.text (string modal.Cantidad)
                                    ]
                                    Html.button [
                                        prop.className "px-2 text-xl font-bold text-rosso"
                                        prop.text "+"
                                        prop.onClick (fun _ -> dispatch (CambiarCantidadModal 1))
                                    ]
                                ]
                            ]
                            Html.button [
                                prop.className "flex-1 rounded-xl bg-rosso py-3 font-extrabold text-white transition active:bg-rosso-dark"
                                prop.onClick (fun _ -> dispatch ConfirmarAgregar)
                                prop.text (sprintf "Agregar · %s" (soles (CartLine.precioLinea (lineaTemporal modal))))
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]
