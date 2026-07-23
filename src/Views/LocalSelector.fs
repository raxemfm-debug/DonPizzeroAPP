// HITO 6.1 — Bottom-sheet de locales: entrega o reserva (deriva a WhatsApp)
module DonPizzero.Views.LocalSelector

open Feliz
open DonPizzero
open DonPizzero.Types
open DonPizzero.State

let private filaLocal (l: Local) (dispatch: Msg -> unit) =
    Html.div [
        prop.key l.Nombre
        prop.className "flex items-center gap-2"
        prop.children [
            Html.button [
                prop.className "flex flex-1 items-center gap-3 rounded-2xl border border-carbon/10 bg-white p-3.5 text-left shadow-sm transition active:scale-[0.99]"
                prop.onClick (fun _ -> dispatch (ElegirLocal l))
                prop.children [
                    Html.span [
                        prop.className "flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-crema text-xl"
                        prop.text "📍"
                    ]
                    Html.p [ prop.className "flex-1 font-extrabold"; prop.text l.Nombre ]
                    Html.span [ prop.className "font-bold text-carbon/40"; prop.text "›" ]
                ]
            ]
            Html.a [
                prop.className "flex h-12 w-12 shrink-0 items-center justify-center rounded-2xl border border-carbon/10 bg-white text-xl shadow-sm transition active:scale-95"
                prop.ariaLabel (sprintf "Ver %s en el mapa" l.Nombre)
                prop.href l.Maps
                prop.target "_blank"
                prop.rel "noreferrer"
                prop.text "🗺️"
            ]
        ]
    ]

let view (modo: ModoSelector) (dispatch: Msg -> unit) =
    Html.div [
        prop.className "fixed inset-0 z-40 flex items-end bg-carbon/50"
        prop.onClick (fun _ -> dispatch CerrarSelectorLocal)
        prop.children [
            Html.div [
                prop.className "max-h-[75vh] w-full overflow-y-auto rounded-t-3xl bg-crema p-5 pb-8"
                prop.onClick (fun e -> e.stopPropagation ())
                prop.children [
                    Html.div [ prop.className "mx-auto mb-4 h-1.5 w-10 rounded-full bg-carbon/20" ]
                    Html.h3 [
                        prop.className "font-display text-2xl text-carbon"
                        prop.text (
                            match modo with
                            | ParaReserva -> "¿En qué local reservas?"
                            | ParaEntrega -> "Elige tu local")
                    ]
                    Html.p [
                        prop.className "mb-4 mt-1 text-sm text-carbon/60"
                        prop.text (
                            sprintf "%d locales en %s · toca 🗺️ para ver el mapa"
                                (List.length Data.negocio.Locales) Data.negocio.Ciudad)
                    ]
                    Html.div [
                        prop.className "flex flex-col gap-2"
                        prop.children [ for l in Data.negocio.Locales -> filaLocal l dispatch ]
                    ]
                ]
            ]
        ]
    ]
