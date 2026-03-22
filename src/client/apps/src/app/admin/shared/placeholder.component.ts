import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-placeholder',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="placeholder-container">
      <div class="placeholder-content">
        <div class="placeholder-icon">{{ icon }}</div>
        <h2>{{ title }}</h2>
        <p>{{ description }}</p>
      </div>
    </div>
  `,
  styles: [
    `
      .placeholder-container {
        display: flex;
        align-items: center;
        justify-content: center;
        min-height: 60vh;
        padding: 2rem;
      }

      .placeholder-content {
        text-align: center;
        max-width: 500px;
      }

      .placeholder-icon {
        font-size: 4rem;
        margin-bottom: 1rem;
      }

      h2 {
        font-size: 2rem;
        margin-bottom: 1rem;
        color: #333;
      }

      p {
        font-size: 1.1rem;
        color: #666;
        line-height: 1.6;
      }
    `,
  ],
})
export class PlaceholderComponent {
  @Input() icon = '🚧';
  @Input() title = 'Coming Soon';
  @Input() description = 'This feature is under development and will be available soon.';
}













