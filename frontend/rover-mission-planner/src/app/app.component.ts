import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TimelineComponent } from './pages/timeline/timeline.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, TimelineComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  roverId: string = '';
  selectedDate: string = new Date().toISOString().split('T')[0];
  searchParams: { roverId: string; date: string } | null = null;

  loadTimeline() {
    if (this.roverId && this.selectedDate) {
      this.searchParams = {
        roverId: this.roverId.trim(),
        date: this.selectedDate,
      };
    }
  }
}
