// HITO 6.1 — Tab Perfil: reserva con selector de local, redes y datos
module DonPizzero.Views.PerfilView

open Feliz
open DonPizzero
open DonPizzero.State

let private icono (emoji: string) =
    Html.span [
        prop.className "flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-crema text-xl"
        prop.text emoji
    ]

let private filaLink (emoji: string) (titulo: string) (url: string) =
    Html.a [
        prop.className "flex items-center gap-3 rounded-2xl border border-carbon/5 bg-white p-3.5 shadow-sm transition active:scale-[0.99]"
        prop.href url
        prop.target "_blank"
        prop.rel "noreferrer"
        prop.children [
            icono emoji
            Html.p [ prop.className "flex-1 font-extrabold"; prop.text titulo ]
            Html.span [ prop.className "font-bold text-carbon/40"; prop.text "›" ]
        ]
    ]

let private filaBoton (emoji: string) (titulo: string) (alPulsar: unit -> unit) =
    Html.button [
        prop.className "flex w-full items-center gap-3 rounded-2xl border border-carbon/5 bg-white p-3.5 text-left shadow-sm transition active:scale-[0.99]"
        prop.onClick (fun _ -> alPulsar ())
        prop.children [
            icono emoji
            Html.p [ prop.className "flex-1 font-extrabold"; prop.text titulo ]
            Html.span [ prop.className "font-bold text-carbon/40"; prop.text "›" ]
        ]
    ]

let private filaInfo (emoji: string) (titulo: string) (detalle: string) =
    Html.div [
        prop.className "flex items-center gap-3 rounded-2xl border border-carbon/5 bg-white p-3.5 shadow-sm"
        prop.children [
            icono emoji
            Html.div [
                prop.className "flex-1"
                prop.children [
                    Html.p [ prop.className "font-extrabold"; prop.text titulo ]
                    Html.p [ prop.className "text-xs text-carbon/60"; prop.text detalle ]
                ]
            ]
        ]
    ]

let private filaPronto (emoji: string) (titulo: string) =
    Html.div [
        prop.className "flex items-center gap-3 rounded-2xl border border-carbon/5 bg-white p-3.5 opacity-60 shadow-sm"
        prop.children [
            icono emoji
            Html.p [ prop.className "flex-1 font-extrabold"; prop.text titulo ]
            Html.span [
                prop.className "rounded-full bg-carbon/10 px-2 py-0.5 text-[10px] font-extrabold uppercase text-carbon/60"
                prop.text "Próximamente"
            ]
        ]
    ]

let view (puntos: int) (dispatch: Msg -> unit) =
    Html.div [
        prop.className "flex flex-col gap-3 px-4 pb-28 pt-4"
        prop.children [
            Html.div [
                prop.className "w-fit rounded-full bg-white px-4 py-2 shadow-sm"
                prop.children [
                    Html.p [
                        prop.className "text-[10px] font-extrabold uppercase tracking-widest text-carbon/50"
                        prop.text "Mi"
                    ]
                    Html.p [
                        prop.className "font-display text-lg leading-none text-carbon"
                        prop.text "Perfil"
                    ]
                ]
            ]

            Html.div [
                prop.className "flex items-center gap-3 rounded-2xl border border-carbon/5 bg-white p-4 shadow-sm"
                prop.children [
                    Html.span [
                        prop.className "flex h-14 w-14 items-center justify-center rounded-full bg-crema text-3xl"
                        prop.text "👤"
                    ]
                    Html.div [
                        prop.children [
                            Html.p [ prop.className "font-extrabold"; prop.text "Sobrino de Don Pizzero" ]
                            Html.p [ prop.className "text-sm text-carbon/60"; prop.text (nivel puntos) ]
                        ]
                    ]
                ]
            ]

            Html.h3 [
                prop.className "mt-1 text-sm font-extrabold uppercase tracking-wide text-carbon/50"
                prop.text "Cuenta"
            ]
            filaBoton "🍽️" "Reservar una mesa" (fun () -> dispatch (AbrirSelectorLocal ParaReserva))
            filaBoton "📍" (sprintf "Nuestros %d locales" (List.length Data.negocio.Locales)) (fun () ->
                dispatch (AbrirSelectorLocal ParaEntrega))
            filaLink "💬" "Escríbenos por WhatsApp" (sprintf "https://wa.me/%s" Data.negocio.WhatsApp)
            filaInfo "🕐" "Horarios" (
                match List.tryHead Data.negocio.Horarios with
                | Some h -> sprintf "%s · %s - %s" h.Dias h.Apertura h.Cierre
                | None -> "")
            filaLink "📘" "Facebook" Data.negocio.Facebook
            filaLink "🎵" "TikTok" Data.negocio.TikTok
            filaPronto "📦" "Mis pedidos"
            filaPronto "💳" "Métodos de pago"
        ]
    ]
