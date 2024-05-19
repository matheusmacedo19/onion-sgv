import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import { NavComponent } from './view/components/templates/nav/nav.component';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MatSlideToggleModule, NavComponent],
  templateUrl:'app.component.html'
})
export class AppComponent {
  title = 'SGV';
}
