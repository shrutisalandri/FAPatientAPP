import { animate, group, query, stagger, state, style, transition, trigger } from '@angular/animations';

export const loginAnimation = trigger('loginAnimation', [
  transition(':enter', [
    style({ transform: 'translateX(-100%)' }),
    animate('1250ms ease-in')
  ])
]);

export const slideIn = trigger('slideIn', [
  state('*', style({
    transform: 'translateX(100%)',
  })),
  state('in', style({
    transform: 'translateX(0)',
  })),
  state('out', style({
    transform: 'translateX(-100%)',
  })),
  transition('* => in', animate('600ms ease-in')),
  transition('in => out', animate('600ms ease-in'))
]);

export const routeAnimation = trigger('routerAnimation', [
  transition('* <=> *', [
    // Initial state of new route
    query(':enter',
      style({
        position: 'fixed',
        width: '100%',
        transform: 'translateX(-100%)'
      }),
      { optional: true }),
    // move page off screen right on leave
    query(':leave',
      animate('500ms ease',
        style({
          position: 'fixed',
          width: '100%',
          transform: 'translateX(100%)'
        })
      ),
      { optional: true }),
    // move page in screen from left to right
    query(':enter',
      animate('500ms ease',
        style({
          opacity: 1,
          transform: 'translateX(0%)'
        })
      ),
      { optional: true }),
  ])
]);

export const fadeInAnimation = trigger('fadeInAnimation', [
  transition('* => *', [
      query('.fade-anim', style({ opacity: 0, transform: 'translateX(-40px)' })),

      query('.fade-anim', stagger('500ms', [
          animate('500ms 1.2s ease-out', style({ opacity: 1, transform: 'translateX(0)' })),
      ])),

      query('.fade-anim', [
          animate(500, style('*'))
      ])

  ])
]);

export const slideInOut = trigger('slideInOut', [
  state('in', style({
    transform: 'translate3d(0, 0, 0)'
  })),
  state('out', style({
    transform: 'translate3d(-104%, 0, 0)'
  })),
  transition('in => out', animate('400ms ease-in-out')),
  transition('out => in', animate('400ms ease-in-out'))
])
