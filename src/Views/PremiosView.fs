// HITO 6 — Tab Premios: Club Don Pizzero (puntos + tarjeta de sellos, demo local)
module DonPizzero.Views.PremiosView

open Feliz
open DonPizzero.State

let private canje (emoji: string) (nombre: string) (detalle: string) (pts: int) =
    Html.div [
        prop.className "flex items-center justify-between rounded-2xl border border-carbon/5 bg-white p-4 shadow-sm"
        prop.children [
            Html.div [
                prop.className "flex items-center gap-3"
                prop.children [
                    Html.span [
                        prop.className "flex h-11 w-11 items-center justify-center rounded-xl bg-crema text-2xl"
                        prop.text emoji
                    ]
                    Html.div [
                        prop.children [
                            Html.p [ prop.className "font-extrabold"; prop.text nombre ]
                            Html.p [ prop.className "text-xs text-carbon/60"; prop.text detalle ]
                        ]
                    ]
                ]
            ]
            Html.span [
                prop.className "rounded-full bg-dorado px-3 py-1 text-xs font-extrabold text-carbon"
                prop.text (sprintf "%d pts" pts)
            ]
        ]
    ]

let view (puntos: int) (sellos: int) =
    Html.div [
        prop.className "flex flex-col gap-4 px-4 pb-28 pt-4"
        prop.children [
            Html.div [
                prop.className "w-fit rounded-full bg-white px-4 py-2 shadow-sm"
                prop.children [
                    Html.p [
                        prop.className "text-[10px] font-extrabold uppercase tracking-widest text-carbon/50"
                        prop.text "Club"
                    ]
                    Html.p [
                        prop.className "font-display text-lg leading-none text-carbon"
                        prop.text "Don Pizzero"
                    ]
                ]
            ]

            // tarjeta de puntos
            Html.div [
                prop.className "rounded-3xl bg-verde p-6 text-center text-white"
                prop.children [
                    Html.p [
                        prop.className "text-xs font-extrabold uppercase tracking-widest text-white/80"
                        prop.text "Tus puntos"
                    ]
                    Html.p [ prop.className "font-display text-5xl"; prop.text (string puntos) ]
                    Html.p [
                        prop.className "mt-1 text-sm font-bold text-white/90"
                        prop.text (sprintf "Nivel %s" (nivel puntos))
                    ]
                ]
            ]

            // tarjeta de sellos
            Html.div [
                prop.className "flex items-center justify-between"
                prop.children [
                    Html.h3 [ prop.className "text-lg font-extrabold"; prop.text "Tu tarjeta de sellos" ]
                    Html.span [ prop.className "font-extrabold text-rosso"; prop.text (sprintf "%d/10" sellos) ]
                ]
            ]
            Html.div [
                prop.className "grid grid-cols-5 justify-items-center gap-3"
                prop.children [
                    for i in 1 .. 10 ->
                        if i <= sellos then
                            Html.div [
                                prop.className "flex h-14 w-14 items-center justify-center rounded-full border-2 border-verde bg-verde/10 text-2xl"
                                prop.text "🍕"
                            ]
                        elif i = 10 then
                            Html.div [
                                prop.className "flex h-14 w-14 items-center justify-center rounded-full border-2 border-dashed border-dorado text-2xl"
                                prop.text "🎁"
                            ]
                        else
                            Html.div [
                                prop.className "flex h-14 w-14 items-center justify-center rounded-full border-2 border-dashed border-carbon/15 font-bold text-carbon/40"
                                prop.text (string i)
                            ]
                ]
            ]
            Html.p [
                prop.className "text-center text-sm font-bold text-carbon/70"
                prop.text (
                    if sellos >= 10 then "¡Tarjeta llena! Tu próxima pizza va por la casa 🎉"
                    else sprintf "¡%d sello(s) más y tu 10.ª pizza va por la casa! 🎉" (10 - sellos))
            ]

            // canjes
            Html.h3 [ prop.className "text-lg font-extrabold"; prop.text "Canjea tus puntos" ]
            canje "🥤" "Gaseosa 1.5L gratis" "Con tu próxima pizza" 120
            canje "🧀" "Borde de queso gratis" "En cualquier pizza" 80
            canje "🍕" "Pizza personal gratis" "La clásica que elijas" 250
            Html.p [
                prop.className "text-center text-xs text-carbon/50"
                prop.text "Muestra esta pantalla al pagar en el local para canjear (beta)."
            ]
        ]
    ]
