import { Component } from '@angular/core';
import { style, state, animate, transition, trigger } from '@angular/core';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    animations: [
        trigger('fadeInOut', [
            transition(':enter', [
                style({ opacity: 0 }),
                animate(100, style({ opacity: 1 }))
            ]),
            transition(':leave', [
                animate(100, style({ opacity: 0 }))
            ])
        ])
    ]
})
export class HomeComponent {
}
